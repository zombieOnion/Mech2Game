using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarWarningLineScript : MonoBehaviour
{
    private float DisappearTimer;
    public NetworkVariable<float> DisappearTimerMax;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        DisappearTimer += Time.deltaTime;
        if (DisappearTimer >= DisappearTimerMax.Value)
        {
            //transform.position = transform.position + Vector3.down * 10;
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true;
        }
    }

    public void ResetTimer() { DisappearTimer = 0; }
}
