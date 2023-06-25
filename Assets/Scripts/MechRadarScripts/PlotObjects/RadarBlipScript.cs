using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarBlipScript : NetworkBehaviour, EnableDisableRendererInterface
{
    public Guid radarSignature;
    public ulong clientID;
    private float DisappearTimer;
    public NetworkVariable<float> DisappearTimerMax;
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
