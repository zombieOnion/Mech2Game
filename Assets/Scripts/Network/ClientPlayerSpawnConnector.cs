using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientPlayerSpawnConnector : NetworkBehaviour
{
    private int role = 0;
    private MechPilotInputConfiguration isMech;
    private EWOInputConfiguration isEwo;

    private void Awake()
    {
        isMech = GetComponent<MechPilotInputConfiguration>();
        isEwo = GetComponent<EWOInputConfiguration>();
    }

    public override void OnNetworkSpawn()
    {
        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsClient)
        {
            var spawner = FindAnyObjectByType<SpawnPlayerManager>();
            var objNet = gameObject.GetComponent<NetworkObject>();
            if (isMech && !isEwo)
            {
                spawner.ClientHasSpawnedPlayerObjectServerRpc(1, objNet);
            }
            else if (!isMech && isEwo)
            {
                spawner.ClientHasSpawnedPlayerObjectServerRpc(2, objNet);
            }
        }
        base.OnNetworkSpawn();
    }
}
