using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadarWarningReceiver : MonoBehaviour
{
    LayerMask radarBlipsMask;
    LayerMask uiMask;
    public GameObject RadarWarningReceiverUI;
    float radarHitTime = 1f;
    public Guid? RadarSignatureOfHit = null;
    float maxRadarReciverTime = 2f;
    float countUpSenseHit = 0f;
    float triggerActivationRestPeriod = 0.2f;
    float triggerActivationRestPeriodCount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        radarBlipsMask = LayerMask.NameToLayer("PLOT");
        uiMask = LayerMask.NameToLayer("UI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != radarBlipsMask || !other.name.Equals("RadarBlip(Clone)"))
            return;
        if(triggerActivationRestPeriodCount < triggerActivationRestPeriod)
            return;
        else
            triggerActivationRestPeriodCount = 0;

        Vector3 hit = new Vector3(other.transform.position.x, 1, other.transform.position.z);
        Vector3 org = new Vector3(transform.position.x, 1, transform.position.z);
        //Debug.Log(hit + " " + org);
        Vector3 dir = (hit - org).normalized;
        //Debug.DrawLine(transform.position, transform.position + dir * 10, Color.red, Mathf.Infinity);
        //Debug.Log(other.name +" "+ dir);
        DrawRadarDirection(dir);
        RadarWarningReceiverUI.SetActive(true);
        RadarSignatureOfHit = (other.GetComponent<RadarBlipScript>()).radarSignature;
        countUpSenseHit = 0;
        //StartCoroutine(TurnOffRWR());
    }

    private void Update()
    {
        countUpSenseHit += Time.deltaTime;
        if (countUpSenseHit > maxRadarReciverTime)
        {
            RadarWarningReceiverUI.SetActive(false);
            countUpSenseHit = 0;
        }
        triggerActivationRestPeriodCount += Time.deltaTime;
    }

    private IEnumerator TurnOffRWR()
    {
        yield return new WaitForSeconds(radarHitTime);
        if (countUpSenseHit > maxRadarReciverTime)
        {
            RadarWarningReceiverUI.SetActive(false);
            countUpSenseHit = 0;
        }
    }

    private void DrawRadarDirection(Vector3 dir)
    {
        GameObject lineGO = new GameObject();
        lineGO.layer = uiMask;
        lineGO.transform.parent = RadarWarningReceiverUI.gameObject.transform;
        lineGO.transform.position = RadarWarningReceiverUI.gameObject.transform.position;
        var currentLineRenderer = lineGO.AddComponent<LineRenderer>();
        currentLineRenderer.startWidth = 3;
        currentLineRenderer.endWidth = 3;
        Vector3 offsetFromText = new Vector3(0, 0, -20);
        currentLineRenderer.SetPositions(new Vector3[] { RadarWarningReceiverUI.transform.position + offsetFromText, RadarWarningReceiverUI.transform.position + offsetFromText + dir * 20 });
        Destroy(lineGO, radarHitTime);
    }
}