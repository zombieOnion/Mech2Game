using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class RadarWarningReceiver : NetworkBehaviour
{
    LayerMask radarBlipsMask;
    LayerMask uiMask;

    public RadarHitList<Transform> LineGos { get; private set; }

    public GameObject RadarWarningReceiverUI;
    float radarHitTime = 1f;
    public Guid? RadarSignatureOfHit = null;
    float maxRadarReciverTime = 2f;
    float countUpSenseHit = 0f;
    float triggerActivationRestPeriod = 0.2f;
    float triggerActivationRestPeriodCount = 0f;
    private GameObjectUtilityFunctions utility;
    private EwoGameObjectReference ewoRefScript = null;
    private Transform canvasGo;
    private GameObject ewoGoId;
    [SerializeField] GameObject RadarWarningLinePreFab;

    // Start is called before the first frame update

    void Awake()
    {
        utility = FindAnyObjectByType<GameObjectUtilityFunctions>();
        ewoRefScript = transform.root.GetComponent<EwoGameObjectReference>();
    }
    void Start()
    {
        radarBlipsMask = LayerMask.NameToLayer("PLOT");
        uiMask = LayerMask.NameToLayer("UI");
    }
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        LineGos = InstantiateRadarLineGeneral(30, 2, transform.position, RadarWarningLinePreFab.transform);
        transform.root.GetComponent<EwoGameObjectReference>().EwoRefeenceId.OnValueChanged += OnEwoGoIdChanged;
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        transform.root.GetComponent<EwoGameObjectReference>().EwoRefeenceId.OnValueChanged -= OnEwoGoIdChanged;
        base.OnNetworkSpawn();
    }

    private void OnEwoGoIdChanged(ulong previous, ulong current)
    {
        canvasGo = utility.FindGameObjectByNetworkObjectId(current).transform.Find("Canvas nav(Clone)");
        ewoGoId = utility.FindGameObjectByNetworkObjectId(current);
        RadarWarningReceiverUI = utility.FindGameObjectByNetworkObjectId(current).transform.Find("Canvas nav(Clone)/RWR").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
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
        if (!IsServer) return;
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
        Vector3 offsetFromText = new Vector3(0, 0, -20);
        var lineGo = LineGos.AdvanceNext();
        lineGo.GetComponent<RadarWarningLineScript>().ResetTimer();
        var currentLineRenderer = lineGo.GetComponent<LineRenderer>();
        currentLineRenderer.SetPositions(new Vector3[] { RadarWarningReceiverUI.transform.position + offsetFromText, RadarWarningReceiverUI.transform.position + offsetFromText + dir * 20 });
        lineGo.GetComponent<NetworkObject>().TrySetParent(canvasGo, true);
    }

    public RadarHitList<Transform> InstantiateRadarLineGeneral(int size, float disappearTime, Vector3 pos, Transform preFab)
    {
        var lobeHits = new RadarHitList<Transform>(size);
        for (int i = 0; i < size; i++)
        {
            var radarHit = Instantiate(preFab, pos, new Quaternion());
            var blipScript = radarHit.gameObject.GetComponent<RadarWarningLineScript>();
            lobeHits.Add(radarHit);
            radarHit.GetComponent<NetworkObject>().Spawn();
            blipScript.DisappearTimerMax.Value = disappearTime;
        }
        return lobeHits;
    }
}