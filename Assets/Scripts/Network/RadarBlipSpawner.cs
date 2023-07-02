using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectPoolSpawner : MonoBehaviour
{
    [SerializeField] private GameObject radarBlipPrefab;
    [SerializeField] private GameObject radarTrackerPrefab;
    // Start is called before the first frame update
    public GameObject SpawnRadarBlip(Vector3 pos, Quaternion rot)
    {
        return SpawnNetworkObject(radarBlipPrefab, pos, rot);
    }

    public GameObject SpawnRadarTrackingObject(Vector3 pos, Quaternion rot)
    {
        return SpawnNetworkObject(radarTrackerPrefab, pos, rot);
    }

    public GameObject SpawnNetworkObject(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(prefab, pos, rot);
        var disScript = obj.GetComponent<DisappearTimerScript>();
        if (disScript != null)
            obj.GetComponent<DisappearTimerScript>().prefab = prefab;
        return obj;
    }
}
