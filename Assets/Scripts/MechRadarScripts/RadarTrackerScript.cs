using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTrackerScript : MonoBehaviour
{
    public Guid MechRadarComputerSignature;
    [SerializeField] public Transform RadarBlip;
    public bool TrackingTarget = false;
    public RadarTargetScript CurrentlyTrackedTarget = null;
    [SerializeField] public LayerMask RadarLayer;
    public RadarMonopulse LeftToRight;
    public RadarMonopulse UpToDown;

    void FixedUpdate() {
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
            return;
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
        if (target.MechRadarComputerSignature != MechRadarComputerSignature)
            return;
        TrackingTarget = true;
        CurrentlyTrackedTarget = target;
        CurrentlyTrackedTarget.TrackerRadarIsOn = true;
        transform.LookAt(target.transform);
    }

    public RadarTargetScript StopTracking()
    {
        TrackingTarget = false;
        CurrentlyTrackedTarget.TrackerRadarIsOn=false;
        var previouslyTrackedTarget = CurrentlyTrackedTarget;
        CurrentlyTrackedTarget = null;
        return previouslyTrackedTarget;
    }
}
