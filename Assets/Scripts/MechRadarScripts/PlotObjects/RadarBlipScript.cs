using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarBlipScript : NetworkBehaviour
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

    void Update()
    {
        DisappearTimer += Time.deltaTime;
        if (DisappearTimer >= DisappearTimerMax.Value)
        {
            //transform.position = transform.position + Vector3.down * 10;
            meshRenderer.enabled = false;
            boxCollider.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
            boxCollider.enabled = true;
        }
    }

    public void ResetAppearTime()
    {
        DisappearTimer = 0;
    }

    [ClientRpc]
    public void ResetAppearTimeClientRpc()
    {
        DisappearTimer = 0;
    }
}
