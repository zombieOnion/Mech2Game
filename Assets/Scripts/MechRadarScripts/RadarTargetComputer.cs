using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PhysicalSpaceLibrary;

public class RadarTargetComputer : MonoBehaviour
{
    public RadarHitList<Transform> HitList;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    public LayerMask PlotLayermask = 1 << 6;
    public LayerMask TargetLayermask = 1 << 8;
    public LayerMask UnitLayermask = 1 << 3;
    private RadarTargetScript _currentlyTrackedTarget = null; //cache variable for hits on the same targetc
    // Start is called before the first frame update
    void Start()
    {
        HitList = new RadarHitList<Transform>(300);
    }

    public void AddRadarHit(Transform hit) {
        HitList.Add(hit);
        Transform radarHit = HitList.GetCurrent();
        if(radarHit == null) {
            _currentlyTrackedTarget = null;
            return;
        }

        Collider[] alreadyExistingRadarTargetsAtHitLocation = CheckForOverlap(radarHit, TargetLayermask);
        bool alreadyMarked = false;
        if(alreadyExistingRadarTargetsAtHitLocation != null && alreadyExistingRadarTargetsAtHitLocation.Length>0) {
            var target = alreadyExistingRadarTargetsAtHitLocation[0];
            // target is already the current tracked target
            if(_currentlyTrackedTarget != null && target.transform.GetInstanceID() == _currentlyTrackedTarget.transform.GetInstanceID()) {
                alreadyMarked = true;
            }
            else {
                foreach(var alreadyPlottedTarget in Targets) {
                    // the target has been seen and tracked earlier, in an earlier sweep cycle, set to current tracked target
                    if(target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID()) {
                        alreadyMarked = true;
                        _currentlyTrackedTarget = target.transform.GetComponent<RadarTargetScript>();
                        var realTarget = CheckForOverlap(radarHit, TargetLayermask);
                        break;
                    }
                }
            }
        }

        
        //Collider[] overlappingPlottHits = CheckForOverlap(radarHit, plotLayermask);
        // if target existed, update it with new radar hit, otherwise create a new radar target, but only if a unity gamebject exist at the hit
        if(alreadyMarked) {
            _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
        }/*
        else if(alreadyMarked == false && overlappingPlottHits.Length > 0) {
            var realTarget = CheckForOverlap(radarHit, unitLayermask);
            if(realTarget != null && realTarget.Length>0 && false) {
                CreateNewTarget(overlappingPlottHits[0].transform.position);
                currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
                return;
            }
        }
        // we hit something but no overlapp with anything, not enough signal to create a target
        */
        else {
            _currentlyTrackedTarget = null;
        }     
    }

    public void DestroyTarget(Transform clickedTarget) {
        Targets.Remove(clickedTarget);
        Destroy(clickedTarget.gameObject);
    }

    public void CreateNewTarget(Vector3 position) {
        Targets.Add(Instantiate(RadarTarget, position, new Quaternion()).transform);
        _currentlyTrackedTarget = Targets[Targets.Count - 1].GetComponent<RadarTargetScript>();
    }

}
