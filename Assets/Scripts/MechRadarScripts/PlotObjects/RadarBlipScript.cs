using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBlipScript : MonoBehaviour, EnableDisableRendererInterface
{
    public Guid radarSignature;
    public ulong CreatorId;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    // Update is called once per frame
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void EnableRenderer()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void DisableRenderer()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }
}
