using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTargetComputer : MonoBehaviour
{
    private RadarSweepScript _radarSweep;
    private RadarTrackerScript _radarTracker;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    public LayerMask PlotLayermask = 1 << 6;
    public LayerMask TargetLayermask = 1 << 8;
    public LayerMask UnitLayermask = 1 << 3;
    // Start is called before the first frame update
    void Start()
    {
        _radarSweep = gameObject.GetComponent<RadarSweepScript>();
        _radarTracker = gameObject.transform.parent.GetComponentInChildren<RadarTrackerScript>();
    }

    public void DestroyTarget(Transform clickedTarget) {/*
        if(clickedTarget.GetInstanceID() == CurrentlyTrackedTarget.GetInstanceID()) {
            TrackingTarget = false;
            OnTarget = false;
            CurrentlyTrackedTarget = null;
        }*/
        if (_radarTracker.TrackingTarget)
            _radarTracker.StopTracking();
        Targets.Remove(clickedTarget);
        Destroy(clickedTarget.gameObject);
    }

    public void CreateNewTarget(Vector3 position) {
        Targets.Add(Instantiate(RadarTarget, position, new Quaternion()).transform);
    }

    public void TrackTarget(RadarTargetScript target) {
        var lockedTarget = Targets.Find(t => t.transform.GetInstanceID() == target.transform.GetInstanceID());
        if (lockedTarget == null)
        {
            var trackedTarget = _radarTracker.StopTracking();
            Targets.Add(trackedTarget.transform);
        }
        else
        {
            Targets.Remove(lockedTarget.transform);
            _radarTracker.TrackTarget(target);
        }
    }
}
