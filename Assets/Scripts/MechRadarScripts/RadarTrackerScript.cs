using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarTrackerScript : NetworkBehaviour
{
    //delegate void Radar();


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
            if(LeftToRight.SearchAndTrack() == 3 && UpToDown.SearchAndTrack() == 3)
            {
                StopTracking();
                return;
            }

            UpdateTargetWithHits(LeftToRight);
            UpdateTargetWithHits(UpToDown);
        }
    }

    private void UpdateTargetWithHits(RadarMonopulse pulse)
    {
        if (CurrentlyTrackedTarget == null)
        {
            StopTracking();
            return;
        }
            
        if (pulse.LobeStraightAhead != null && pulse.LobeStraightAhead.Length > 0)
        {
            foreach (var hit in pulse.LobeStraightAhead)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit);
        }
        if (pulse.LobeHitsLeft != null && pulse.LobeHitsLeft.Length > 0)
        {
            foreach(var hit in pulse.LobeHitsLeft)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit);
        }
        if(pulse.LobeHitsRight != null && pulse.LobeHitsRight.Length > 0)
        {
            foreach (var hit in pulse.LobeHitsRight)
                CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(hit);
        }
    }

    public void TrackTarget(RadarTargetScript target) {
        //if (target.MechRadarComputerSignature != MechRadarComputerSignature)
        //    return;
        LeftToRight.CreateaBlipCache();
        UpToDown.CreateaBlipCache();
        TrackingTarget = true;
        CurrentlyTrackedTarget = target;
        CurrentlyTrackedTarget.TrackerRadarIsOn = true;
        CurrentlyTrackedTarget.OnRadarTargetDestroyed += StopTracking;
        transform.LookAt(target.transform);
        Debug.Log("Started Tracking");
    }

    public void StopTracking()
    {
        TrackingTarget = false;
        if (CurrentlyTrackedTarget == null)
            return;
        CurrentlyTrackedTarget.TrackerRadarIsOn = false;
        var previouslyTrackedTarget = CurrentlyTrackedTarget;
        CurrentlyTrackedTarget = null;
        CleanupAfterStopTracking();
        Debug.Log("Stoped Tracking");
        //return previouslyTrackedTarget;
    }

    private void CleanupAfterStopTracking()
    {
        LeftToRight.DestroyBlipCache();
        UpToDown.DestroyBlipCache();
    }

    public override void OnDestroy()
    {
        if (CurrentlyTrackedTarget != null)
        {
            CleanupAfterStopTracking();
            Destroy(CurrentlyTrackedTarget.gameObject);
        }
        base.OnDestroy();
    }
}
