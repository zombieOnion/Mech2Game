using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AreaTriggerScript : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (!other.name.StartsWith("PlayerMech"))
            return;
        NetworkObject.Despawn(true);
    }

    public override void OnDestroy()
    {
        if (!IsServer)
            return;
        print("Trigger destroyed");
        base.OnDestroy();
    }
}
