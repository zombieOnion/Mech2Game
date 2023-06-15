using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MoveForward : NetworkBehaviour
{
    public float thrust;
    public Rigidbody rb;
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            rb.AddForce(rb.transform.forward * thrust, ForceMode.VelocityChange);
        }
    }
}
