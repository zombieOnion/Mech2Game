using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MissionControllerScript : NetworkBehaviour
{
    //public delegate void MissionStageAssetDestroyed();
    //public event MissionStageAssetDestroyed OnMissionStageAssetDestroyed;
    public MissionStageScript[] missionStageScripts;
    private NetworkObjectPoolSpawner spawner;
    private bool playerSpawningIsDone;
    private int currentMissionStageIndex = 0;
    private TextMeshProUGUI textComponent;

    public static MissionControllerScript Singleton { get; private set; }
    public void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }
    public override void OnNetworkSpawn()
    {
        var textGo = GameObject.Find("Canvas/MissionTextGo");
        textComponent = textGo.GetComponent<TextMeshProUGUI>();
        if (!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        spawner = NetworkObjectPoolSpawner.Singleton;

        SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged += OnAllPlayerSpawnedValueChanged;
        base.OnNetworkDespawn();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged -= OnAllPlayerSpawnedValueChanged;
            base.OnNetworkDespawn();
            return;
        }
        base.OnNetworkDespawn();
    }

    private void OnAllPlayerSpawnedValueChanged(bool oldValue, bool newValue)
    {
        playerSpawningIsDone = newValue;
        currentMissionStageIndex = 0;
        var currentmss = missionStageScripts[0];
        for (int i = 0; i < currentmss.gosToSpawn.Length; i++)
        {
            SpawnNgo(currentmss.gosToSpawn[i]);
        }
        textComponent.text = currentmss.missionText;
        if (textComponent.text != null)
            SetMissionTextClientRpc(textComponent.text);
        currentmss.hasBeenActivated = true;
    }

    public void MissionStageCallback(int nextIndex)
    {
        if (nextIndex < 1) return;
        currentMissionStageIndex = nextIndex;
        if(nextIndex >= missionStageScripts.Length)
        {
            Debug.Log("No next index: " + nextIndex);
            return;
        }
        var currentStage = missionStageScripts[currentMissionStageIndex];
        if (currentStage.hasBeenActivated) return;
        SpawnNgos(currentStage);
        textComponent.text = currentStage.missionText;
        if(textComponent.text != null)
            SetMissionTextClientRpc(textComponent.text);
        currentStage.hasBeenActivated = true;
    }

    [ClientRpc] 
    public void SetMissionTextClientRpc(string newMissionText)
    {
        textComponent.text = newMissionText;
    }

    private void SpawnNgos(MissionStageScript currentmss)
    {
        for (int i = 0; i < currentmss.gosToSpawn.Length; i++)
        {
            SpawnNgo(currentmss.gosToSpawn[i]);
        }
    }
    public void SpawnNgo(GoSpawnInfo gsi)
    {
        var newlySpawned = spawner.InstantiateNetworkObject(gsi.Prefab, gsi.position, gsi.rotation);
        var callbackScript = newlySpawned.GetComponent<ChangeUiTextOnDesttroy>();
        callbackScript.MissionIndex = gsi.nextMissionStage;
        newlySpawned.GetComponent<NetworkObject>().Spawn();
    }

}

[Serializable]
public struct GoSpawnInfo
{
    public GameObject Prefab;
    public Vector3 position;
    public Quaternion rotation;
    public int nextMissionStage;
}

[Serializable]
public class MissionStageScript
{
    public GoSpawnInfo[] gosToSpawn;
    public string missionText;
    public bool hasBeenActivated;
}