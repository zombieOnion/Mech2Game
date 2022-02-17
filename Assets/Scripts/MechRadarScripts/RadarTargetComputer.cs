using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PhysicalSpaceLibrary;

public class RadarTargetComputer : MonoBehaviour
{
    private RadarSweepScript _radarSweep;
    private RadarTrackerScript _radarTracker;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    public LayerMask PlotLayermask = 1 << 6;
    public LayerMask TargetLayermask = 1 << 8;
    public LayerMask UnitLayermask = 1 << 3;
    private RadarTargetScript _currentlyTrackedTarget = null; //cache variable for hits on the same targetc
    // Start is called before the first frame update
    void Start()
    {
        _radarSweep = gameObject.GetComponent<RadarSweepScript>();
        _radarTracker = gameObject.transform.parent.GetComponentInChildren<RadarTrackerScript>();
    }

    public void AddRadarHit(Transform radarHit) {
        if(radarHit == null) {
            _currentlyTrackedTarget = null;
            return;
        }

        Collider[] alreadyExistingRadarTargetsAtHitLocation = CheckForOverlap(radarHit, TargetLayermask);
        if(alreadyExistingRadarTargetsAtHitLocation != null && alreadyExistingRadarTargetsAtHitLocation.Length>0) {
            var target = alreadyExistingRadarTargetsAtHitLocation[0];
            // if already tracking target don't look it up in the targetList again
            if(_currentlyTrackedTarget != null && target.transform.GetInstanceID() == _currentlyTrackedTarget.transform.GetInstanceID()) {
                _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
            }
            else {
                foreach(var alreadyPlottedTarget in Targets) {
                    if(target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID()) {
                        _currentlyTrackedTarget = target.transform.GetComponent<RadarTargetScript>(); //cache tracked target
                        _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
                        break;
                    }
                }
            }
        }
        //hit something but no overlapp with anything, not enough signal to create a target
        else {
            _currentlyTrackedTarget = null;
        }     
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
        _currentlyTrackedTarget = Targets[Targets.Count - 1].GetComponent<RadarTargetScript>();
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
