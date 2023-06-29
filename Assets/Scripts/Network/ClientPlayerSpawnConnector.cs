using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientPlayerSpawnConnector : NetworkBehaviour
{
    private int role = 0;
    private MechPilotInputConfiguration isMech;
    private EWOInputConfiguration isEwo;
    [SerializeField] public NetworkVariable<int> team;

    private void Awake()
    {
        isMech = GetComponent<MechPilotInputConfiguration>();
        isEwo = GetComponent<EWOInputConfiguration>();
    }

    public override void OnNetworkSpawn(){
        if (IsClient)
            team.OnValueChanged += OnTeamSet;
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        if (IsClient)
            team.OnValueChanged -= OnTeamSet;
        base.OnNetworkDespawn();
    }

    public void OnTeamSet(int oldValue, int newValue)
    {
        if (!IsClient) return;
        var spawner = FindAnyObjectByType<SpawnPlayerManager>();
        var objNet = gameObject.GetComponent<NetworkObject>();
        if (isMech && !isEwo)
        {
            spawner.ClientHasSpawnedPlayerObjectServerRpc(1, newValue, objNet);
        }
        else if (!isMech && isEwo)
        {
            spawner.ClientHasSpawnedPlayerObjectServerRpc(2, newValue, objNet);
        }
    }
}
