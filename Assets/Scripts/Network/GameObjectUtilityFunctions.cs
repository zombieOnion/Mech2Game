using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameObjectUtilityFunctions : MonoBehaviour
{

    public List<Transform> FindNewPlayerComponents<scriptType>(ulong clientId, List<scriptType> existingList) where scriptType : UnityEngine.Component
    {
        //var playerGameObjects = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(networkObjectId);
        var playerGameObjects = FindObjectsOfType<scriptType>().Select(rts => rts.gameObject).Where(go => go.GetComponent<NetworkObject>().OwnerClientId == clientId).ToList();
        var playerGameObjectsFiltered = FindObjectsOfType<scriptType>().Select(rts => rts.gameObject).Where(go => go.GetComponent<NetworkObject>().OwnerClientId == clientId).ToList();
        var playerGameObjectsNetworkIds = playerGameObjects.Select(x => x.GetComponent<NetworkObject>().NetworkObjectId).ToList();
        Debug.Log($"radars: {playerGameObjects.Count()}");
        var targetIDs = existingList.Select(t => t.gameObject.GetComponent<NetworkObject>().NetworkObjectId).ToList();
        var newTargets = playerGameObjects.Where(x => !targetIDs.Contains(x.GetComponent<NetworkObject>().NetworkObjectId)).ToList();
        Debug.Log($"new radars: {newTargets.Count()}");
        return newTargets.Select(g => g.transform).ToList();
    }

    public List<ScriptType> FindNewPlayerObjectsClient<ScriptType>(ulong clinetId) where ScriptType : UnityEngine.Component
    {
        //var playerGameObjects = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(networkObjectId);
        var playerGameObjects = FindObjectsOfType<ScriptType>().
            Where(go => go.GetComponent<NetworkObject>().OwnerClientId == clinetId).ToList();
        return playerGameObjects;
    }
    

    public GameObject FindTypeByNetworkId<T>(ulong networkID) where T : UnityEngine.Component
    {
        var foundGo = FindObjectsOfType<T>().Select(rts => rts.gameObject);
        var foundGoNetworkId = foundGo.First(x => x.GetComponent<NetworkObject>().NetworkObjectId == networkID);
        return foundGoNetworkId;
    }

    public GameObject FindGameObjectByNetworkObjectId(ulong networkObjectId)
    {
        return NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].gameObject;
    }

    public GameObject FindPlayerGameObjectByNetworkObjectId(ulong networkObjectId, ulong clinetId)
    {
        var playersNetworkObject = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(clinetId);
        var networkObject = FindGameObjectByNetworkObjectId(networkObjectId);
        var networkObjId = networkObject.GetComponent<NetworkObject>().NetworkObjectId;
        return playersNetworkObject.FirstOrDefault(pno => pno.NetworkObjectId == networkObjectId)?.gameObject;
    }
}
