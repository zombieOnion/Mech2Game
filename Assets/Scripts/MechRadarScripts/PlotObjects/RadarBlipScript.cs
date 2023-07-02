using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarBlipScript : NetworkBehaviour, EnableDisableRendererInterface
{
    public Guid radarSignature;
    public ulong clientID;
    public ulong CreatorId;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    public GameObject prefab;

    // Update is called once per frame
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    public override void OnNetworkSpawn()
    {
        clientID = GetComponent<NetworkObject>().OwnerClientId;
        base.OnNetworkSpawn();
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
