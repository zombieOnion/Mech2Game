using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTargetComputer : MonoBehaviour
{
    public RadarHitList HitList;
    public List<Transform> Targets = new List<Transform>();
    [SerializeField] public Transform RadarTarget;
    private LayerMask plotLayermask = 1 << 6;
    private LayerMask targetLayermask = 1 << 8;
    private LayerMask unitLayermask = 1 << 3;
    private RadarTargetScript currentlyTrackedTarget = null; //cache variable for hits on the same targetc
    // Start is called before the first frame update
    void Start()
    {
        HitList = new RadarHitList(300);
    }

    public void AddRadarHit(Transform hit) {
        HitList.Add(hit);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Transform radarHit = HitList.GetCurrent();
        if(radarHit == null) {
            currentlyTrackedTarget = null;
            return;
        }

        Collider[] alreadyExistingRadarTargetsAtHitLocation = CheckForOverlap(radarHit, targetLayermask);
        bool alreadyMarked = false;
        if(alreadyExistingRadarTargetsAtHitLocation != null && alreadyExistingRadarTargetsAtHitLocation.Length>0) {
            var target = alreadyExistingRadarTargetsAtHitLocation[0];
            // target is already the current tracked target
            if(currentlyTrackedTarget != null && target.transform.GetInstanceID() == currentlyTrackedTarget.transform.GetInstanceID()) {
                alreadyMarked = true;
            }
            else {
                foreach(var alreadyPlottedTarget in Targets) {
                    // the target has been seen and tracked earlier, in an earlier sweep cycle, set to current tracked target
                    if(target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID()) {
                        alreadyMarked = true;
                        currentlyTrackedTarget = target.transform.GetComponent<RadarTargetScript>();
                        var realTarget = CheckForOverlap(radarHit, targetLayermask);
                        break;
                    }
                }
            }
        }

        Collider[] overlappingPlottHits = CheckForOverlap(radarHit, plotLayermask);
        // if target existed, update it with new radar hit, otherwise create a new radar target, but only if a unity gamebject exist at the hit
        if(alreadyMarked) {
            currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
        }
        else if(alreadyMarked == false && overlappingPlottHits.Length > 0) {
            var realTarget = CheckForOverlap(radarHit, unitLayermask);
            if(realTarget != null && realTarget.Length>0) {
                Targets.Add(Instantiate(RadarTarget, overlappingPlottHits[0].transform.position, new Quaternion()).transform);
                currentlyTrackedTarget = Targets[Targets.Count - 1].GetComponent<RadarTargetScript>();
                currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
                currentlyTrackedTarget.TargetTransform = realTarget[0].gameObject.transform;
            }
        }
        // we hit something but no overlapp with anything, not enough signal to create a target
        else {
            currentlyTrackedTarget = null;
        }        
    }

    private Collider[] CheckForOverlap(Transform transform, LayerMask layer) => Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, layer);
}
