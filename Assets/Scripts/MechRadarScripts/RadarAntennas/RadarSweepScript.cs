using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PhysicalSpaceLibrary;

public class RadarSweepScript : MonoBehaviour
{
    // general variables
    private Transform radarSweepTransform;
    [SerializeField] public LayerMask RadarLayer;
    private Transform mechTransform;
    private RadarTargetComputer targetProcessor;
    private bool radarOn = true;
    public bool RadarOn { get => radarOn; set => radarOn = value; }
    private RadarTargetScript _currentlyTrackedTarget = null; //cache variable for hits on the same targetc

    // cache variables and blips
    [SerializeField] public Transform RadarBlip;
    public RadarHitList<Transform> HitList;
    private int blipCount = 1000;
    private float blipTimeOut = 5;

    // radar rotation, speed and distance variable
    private Collider radarSweepCollider;
    private float xSweepRotationAngle;
    public float radarDistance = 500;
    public float SweepSpeed;
    public float SweepSpeedChangeNumber = 60f;
    public int MaxSweepSpeed = 360;
    public int MinimumSweepSpeed = 30;

    // sector sweep and change direction

    
    // Start is called before the first frame update
    void Awake()
    {
        radarSweepTransform = transform;
        radarSweepCollider = radarSweepTransform.GetComponent<Collider>();
        mechTransform = transform.parent.parent;
        targetProcessor = gameObject.GetComponent<RadarTargetComputer>();
    }

    void Start()
    {
        HitList = new RadarHitList<Transform>(blipCount);
        for (int i = 0; i < blipCount; i++)
        {
            var radarblip = Instantiate(RadarBlip, transform.position + Vector3.down*5, new Quaternion());
            radarblip.GetComponent<RadarBlipScript>().DisappearTimerMax = blipTimeOut;
            HitList.Add(radarblip);
        }
    }

    public void FixedUpdate()
    {
        if (!radarOn)
            return;
        xSweepRotationAngle -= Time.fixedDeltaTime * SweepSpeed;
        radarSweepTransform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        var hits = SendAndRecieveRadarPulse(HitList);
        if (hits.Length == 0)
            return;
        foreach (var hit in hits.Select(hit => hit.transform).ToArray())
            AddRadarHit(hit);
    }

    private RaycastHit[] SendAndRecieveRadarPulse(RadarHitList<Transform> blipPool)
    {
        RaycastHit[] hits = Physics.BoxCastAll(radarSweepCollider.bounds.center, radarSweepTransform.localScale, radarSweepTransform.forward, radarSweepTransform.rotation, radarDistance, RadarLayer); 
        if (hits.Length < 1)
            return new RaycastHit[0];
        foreach (RaycastHit hit in hits) {
           if(hit.distance != 0) {
                var nextHit = blipPool.AdvanceNext();
                var nextHitScript = nextHit.GetComponent<RadarBlipScript>();
                nextHitScript.gameObject.SetActive(true);
                nextHitScript.ResetAppearTime();
                nextHit.position = hit.point;
                nextHit.rotation = Quaternion.identity;
           }
        }
        return hits.Where(hit => hit.distance > 5).ToArray();
    }

    public void AddRadarHit(Transform radarHit)
    {
        Collider[] alreadyExistingRadarTargetsAtHitLocation = CheckForOverlap(radarHit, targetProcessor.TargetLayermask);
        if (alreadyExistingRadarTargetsAtHitLocation != null && alreadyExistingRadarTargetsAtHitLocation.Length > 0)
        {
            var target = alreadyExistingRadarTargetsAtHitLocation[0];
            // if already tracking target don't look it up in the targetList again
            if (_currentlyTrackedTarget != null && target.transform.GetInstanceID() == _currentlyTrackedTarget.transform.GetInstanceID())
            {
                _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
            }
            else
            {
                foreach (var alreadyPlottedTarget in targetProcessor.Targets)
                {
                    if (target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID())
                    {
                        _currentlyTrackedTarget = target.transform.GetComponent<RadarTargetScript>(); //cache tracked target
                        _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
                        break;
                    }
                }
            }
        }
        //hit something but no overlapp with anything, not enough signal to create a target
        else
        {
            _currentlyTrackedTarget = null;
        }
    }

    /*void FixedUpdate() {
        radarSweepTransform.transform.position = new Vector3(mechTransform.position.x, mechTransform.position.y+1, transform.position.z);
    }*/

    public void ChangeSweepDirection() {
        SweepSpeed *= -1;
    }

    public void IncreaseSweepSpeed() {
        if (MaxSweepSpeed <= SweepSpeed)
            return;
        else
            SweepSpeed += SweepSpeedChangeNumber;
    }

    public void DecreaseSweepSpeed() {
        if (MinimumSweepSpeed >= SweepSpeed)
            return;
        else
            SweepSpeed -= SweepSpeedChangeNumber;
    }


}
