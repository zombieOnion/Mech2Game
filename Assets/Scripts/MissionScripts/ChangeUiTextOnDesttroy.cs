using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChangeUiTextOnDesttroy : NetworkBehaviour
{
    public GoSpawnInfo[] ActivateUiElements;
    public int MissionIndex = 0;
    private NetworkObjectPoolSpawner spawner;
    private MissionControllerScript missionController;

    public override void OnNetworkSpawn()
    {
        spawner = NetworkObjectPoolSpawner.Singleton;
        missionController = MissionControllerScript.Singleton;
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;
        missionController.MissionStageCallback(MissionIndex);
        base.OnNetworkDespawn();
    }

    public override void OnDestroy()
    {
        if (!IsServer || !IsHost) return;
        //missionController.MissionStageCallback(MissionIndex);
        base.OnDestroy();
    }
}
