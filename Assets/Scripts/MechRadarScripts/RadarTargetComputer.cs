using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarTargetComputer : NetworkBehaviour
{
    private Guid mechRadarComputerSignature;
    private GameObjectUtilityFunctions _utility;
    private RadarTrackerScript _radarTracker;
    private JammerScript _jammer;
    private RadarWarningReceiver _radarWarningReceiver;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    public LayerMask PlotLayermask = 1 << 6;
    public LayerMask TargetLayermask = 1 << 8;
    public LayerMask UnitLayermask = 1 << 3;
    private NetworkObjectPoolSpawner spawner;

    public Guid MechRadarComputerSignature
    {
        get => mechRadarComputerSignature; 
        set
        {
            mechRadarComputerSignature = value;
            _radarTracker.MechRadarComputerSignature = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _utility = transform.root.GetComponent<GameObjectUtilityFunctions>();
        _radarTracker = gameObject.transform.parent.GetComponentInChildren<RadarTrackerScript>();
        _radarWarningReceiver = gameObject.transform.parent.GetComponentInChildren<RadarWarningReceiver>();
        _jammer = gameObject.GetComponent<JammerScript>();
    }

    void Start()
    {
        _radarTracker.MechRadarComputerSignature = MechRadarComputerSignature;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            base.OnNetworkDespawn();
            return;
        }
        spawner = FindAnyObjectByType<NetworkObjectPoolSpawner>();
        base.OnNetworkDespawn();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            base.OnNetworkDespawn();
            return;
        }
        Targets.ForEach(x => Destroy(x.gameObject));
        Targets.Clear();
        base.OnNetworkDespawn();
    }

    public void DestroyTarget(Transform clickedTarget) {
        if (_radarTracker.TrackingTarget && _radarTracker.CurrentlyTrackedTarget != null &
            clickedTarget.GetComponent<NetworkObject>().NetworkObjectId ==
            _radarTracker.CurrentlyTrackedTarget.GetComponent<NetworkObject>().NetworkObjectId)
            _radarTracker.StopTracking();
        Targets.Remove(clickedTarget);
        Destroy(clickedTarget.gameObject);
    }

    public List<Transform> FindNewPlayerTargets(ulong networkObjectId)
    {
        //var playerGameObjects = NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(networkObjectId);
        var playerGameObjects = FindObjectsOfType<RadarTargetScript>().Select(rts => rts.gameObject).ToList();
        var playerGameObjectsNetworkIds = playerGameObjects.Select(x => x.GetComponent<NetworkObject>().NetworkObjectId).ToList();
        Debug.Log($"radars: {playerGameObjects.Count()}");
        var targetIDs = Targets.Select(t => t.gameObject.GetComponent<NetworkObject>().NetworkObjectId).ToList();
        var newTargets = playerGameObjects.Where(x => !targetIDs.Contains(x.GetComponent<NetworkObject>().NetworkObjectId)).ToList();
        Debug.Log($"new radars: {newTargets.Count()}");
        return newTargets.Select(g => g.transform).ToList();
    }

    public GameObject CreateNewTarget(Vector3 position, ulong clientId, bool isServer = false) {
        var newTarget = spawner.InstansiateRadarTrackingObject(position, Quaternion.identity);
        newTarget.GetComponent<RadarTargetScript>().MechRadarComputerSignature = MechRadarComputerSignature;
        //newTarget.gameObject.GetComponent<NetworkObject>().Spawn();
        newTarget.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        Targets.Add(newTarget.transform);
        return newTarget.gameObject;
    }

    public void TrackTarget(RadarTargetScript target) {
        //var lockedTarget = Targets.Find(t => t.transform.GetInstanceID() == target.transform.GetInstanceID());
        if (target == null)
        {
            _radarTracker.StopTracking();
            //var trackedTarget = _radarTracker.StopTracking();
            //if (trackedTarget != null)
            //    Targets.Add(trackedTarget.transform);
        }
        else
        {
            Targets.Remove(target.transform);
            _radarTracker.TrackTarget(target);
        } 
    }

    public void JammTarget()
    {
        if (!_jammer.IsJamming)
        {
            _jammer.IsJamming = true;
            _jammer.SetUpJamming(_radarWarningReceiver.RadarSignatureOfHit, _radarTracker.CurrentlyTrackedTarget);
        } else {
            _jammer.IsJamming = false;
        }
    }

}
