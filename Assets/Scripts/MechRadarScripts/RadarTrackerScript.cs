using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTrackerScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public bool TrackingTarget = false;
    public RadarTargetScript CurrentlyTrackedTarget = null;
    [SerializeField] public LayerMask RadarLayer;
    public RadarMonopulse LeftToRight;
    public RadarMonopulse UpToDown;

    void Update() {
        if (TrackingTarget)
        {
            LeftToRight.SendMonoPulse();
            UpToDown.SendMonoPulse();
            UpdateTargetWithHits(LeftToRight);
            UpdateTargetWithHits(UpToDown);
        }
    }

    private void UpdateTargetWithHits(RadarMonopulse pulse)
    {
         if (pulse.LobeHitLeft != null)
            CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(pulse.LobeHitLeft.Value.transform);
         else if (pulse.LobeHitRight != null)
            CurrentlyTrackedTarget.ReceiveNewRadarHitOnTarget(pulse.LobeHitRight.Value.transform);
    }

    public void TrackTarget(RadarTargetScript target) {
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
