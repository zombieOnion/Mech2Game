using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarTrackerScript : NetworkBehaviour
{
    public Guid MechRadarComputerSignature;
    [SerializeField] public Transform RadarBlip;
    public bool TrackingTarget = false;
    public RadarTargetScript CurrentlyTrackedTarget = null;
    [SerializeField] public LayerMask RadarLayer;
    public RadarMonopulse LeftToRight;
    public RadarMonopulse UpToDown;
    private GameObjectUtilityFunctions _utility;

    private void Awake()
    {
        _utility = transform.root.GetComponent<GameObjectUtilityFunctions>();
    }

    void FixedUpdate() {
        if (!IsServer) return;
        if (TrackingTarget)
        {
            LeftToRight.SearchAndTrack();
            UpToDown.SearchAndTrack();
            UpdateTargetWithHits(LeftToRight);
            UpdateTargetWithHits(UpToDown);
        }
    }

    private void UpdateTargetWithHits(RadarMonopulse pulse)
    {
        if (CurrentlyTrackedTarget == null)
        {
            TrackingTarget = false;
            return;
        }
            
        if (pulse.LobeStraightAhead != null && pulse.LobeStraightAhead.Length > 0)
        {
            foreach (var hit in pulse.LobeStraightAhead)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit.transform);
        }
        if (pulse.LobeHitsLeft != null && pulse.LobeHitsLeft.Length > 0)
        {
            foreach(var hit in pulse.LobeHitsLeft)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit.transform);
        }
        if(pulse.LobeHitsRight != null && pulse.LobeHitsRight.Length > 0)
        {
            foreach (var hit in pulse.LobeHitsRight)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit.transform);
        }
    }

    public void TrackTarget(RadarTargetScript target) {
        //if (target.MechRadarComputerSignature != MechRadarComputerSignature)
        //    return;
        LookAtNewTargetServerRpc(target.GetComponent<NetworkObject>().NetworkObjectId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LookAtNewTargetServerRpc(ulong targetNetObId, ServerRpcParams serverRpcParams = default)
    {
        var targetGo = _utility.FindPlayerGameObjectByNetworkObjectId(targetNetObId, serverRpcParams.Receive.SenderClientId);
        var target = targetGo.GetComponent<RadarTargetScript>();
        TrackingTarget = true;
        CurrentlyTrackedTarget = target;
        CurrentlyTrackedTarget.TrackerRadarIsOn = true;
        transform.LookAt(target.transform);
    }

    public RadarTargetScript StopTracking()
    {
        TrackingTarget = false;
        if (CurrentlyTrackedTarget == null)
            return null;
        CurrentlyTrackedTarget.TrackerRadarIsOn=false;
        var previouslyTrackedTarget = CurrentlyTrackedTarget;
        CurrentlyTrackedTarget = null;
        return previouslyTrackedTarget;
    }

    [ServerRpc]
    public void StopTrackingServerRpc()
    {
        TrackingTarget = false;
        if (CurrentlyTrackedTarget == null)
            return;
        CurrentlyTrackedTarget.TrackerRadarIsOn = false;
        var previouslyTrackedTarget = CurrentlyTrackedTarget;
        CurrentlyTrackedTarget = null;
        return;
    }

    public void OnDestroy()
    {
        Destroy(CurrentlyTrackedTarget.gameObject);
        base.OnDestroy();
    }
}
