using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarWarningLineScript : MonoBehaviour, EnableDisableRendererInterface
{
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void EnableRenderer()
    {
        lineRenderer.enabled = true;
    }

    public void DisableRenderer()
    {
        lineRenderer.enabled = false;
    }

    public void ResetPos(Vector3 start, Vector3 end)
    {
        var currentLineRenderer = gameObject.GetComponent<LineRenderer>();
        currentLineRenderer.SetPositions(new Vector3[] { start, end });
    }
}
