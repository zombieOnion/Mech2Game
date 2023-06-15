using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarTargetComputer : NetworkBehaviour
{
    public Guid MechRadarComputerSignature;
    private RadarTrackerScript _radarTracker;
    private JammerScript _jammer;
    private RadarWarningReceiver _radarWarningReceiver;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    public LayerMask PlotLayermask = 1 << 6;
    public LayerMask TargetLayermask = 1 << 8;
    public LayerMask UnitLayermask = 1 << 3;
    public bool IsJamming { private set; get; }
    // Start is called before the first frame update
    void Awake()
    {
        _radarTracker = gameObject.transform.parent.GetComponentInChildren<RadarTrackerScript>();
        _radarWarningReceiver = gameObject.transform.parent.GetComponentInChildren<RadarWarningReceiver>();
        _jammer = gameObject.GetComponent<JammerScript>();
    }

    void Start()
    {
        _radarTracker.MechRadarComputerSignature = MechRadarComputerSignature;
    }

    public void DestroyTarget(Transform clickedTarget) {
        if (_radarTracker.TrackingTarget)
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

    public GameObject FindGameObjectByNetworkObjectId(ulong networkObjectId)
    {
        return NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].gameObject;
    }

    public void CreateNewTarget(Vector3 position) {
        CreateNewTargetServerRpc(position);
        var newTargets = FindNewPlayerTargets(NetworkManager.LocalClient.ClientId);
        Targets.AddRange(newTargets);
        //return newTarget.GetComponent<RadarTargetScript>();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateNewTargetServerRpc(Vector3 position, ServerRpcParams serverRpcParams = default)
    {
        var newTarget = Instantiate(RadarTarget, position, new Quaternion());
        newTarget.GetComponent<RadarTargetScript>().MechRadarComputerSignature = MechRadarComputerSignature;
        var clientId = serverRpcParams.Receive.SenderClientId;
        newTarget.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }

    public void TrackTarget(RadarTargetScript target) {
        var lockedTarget = Targets.Find(t => t.transform.GetInstanceID() == target.transform.GetInstanceID());
        if (lockedTarget == null)
        {
            var trackedTarget = _radarTracker.StopTracking();
            if(trackedTarget != null)
                Targets.Add(trackedTarget.transform);
        }
        else
        {
            Targets.Remove(lockedTarget.transform);
            _radarTracker.TrackTarget(target);
        }
    }

    public void JammTarget()
    {
        if (!IsJamming)
        {
            _jammer.IsJamming = true;
            _jammer.SetUpJamming(_radarWarningReceiver.RadarSignatureOfHit, _radarTracker.CurrentlyTrackedTarget);
            IsJamming = true;
        } else {
            _jammer.IsJamming = false;
            IsJamming = false;
        }
    }

    public void OnDestroy()
    {
        Targets.ForEach(x => Destroy(x.gameObject));
        Targets.Clear();
    }
}
