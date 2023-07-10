using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkObjectPoolSpawner : MonoBehaviour
{
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

    //[SerializeField] private GameObject radarBlipPrefab;
    [SerializeField] private GameObject radarTrackerPrefab;

    public static NetworkObjectPoolSpawner Singleton { get; private set; }

    // Start is called before the first frame update
    /*public GameObject SpawnRadarBlip(Vector3 pos, Quaternion rot)
    {
        return SpawnNetworkObject(radarBlipPrefab, pos, rot);
    }*/

    public GameObject InstansiateRadarTrackingObject(Vector3 pos, Quaternion rot)
    {
        return InstantiateNetworkObject(radarTrackerPrefab, pos, rot);
    }

    public GameObject InstantiateNetworkObject(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(prefab, pos, rot);
        var disScript = obj.GetComponent<DisappearTimerScript>();
        if (disScript != null)
            obj.GetComponent<DisappearTimerScript>().prefab = prefab;
        return obj;
    }
}
