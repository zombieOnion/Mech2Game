using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PhysicalSpaceLibrary;

public class RadarSweepScript : MonoBehaviour
{
    // general variables
    [SerializeField] public LayerMask RadarLayer;
    private RadarTargetComputer targetProcessor;
    private bool radarOn = true;
    public bool RadarOn { get => radarOn; set => radarOn = value; }
    private RadarTargetScript _currentlyTrackedTarget = null; //cache variable for hits on the same targetc

    // cache variables and blips
    public SendRadarPulseAndCreateRadarEchoes PulseSender;
    public RadarHitList<Transform> HitList;
    private int blipCount = 1000;
    private float blipTimeOut = 5;

    // radar rotation, speed and distance variable
    private float xSweepRotationAngle;
    public float SweepSpeed;
    public float SweepSpeedChangeNumber = 60f;
    public int MaxSweepSpeed = 360;
    public int MinimumSweepSpeed = 30;

    // sector sweep and change direction

    
    // Start is called before the first frame update
    void Awake()
    {
        targetProcessor = gameObject.GetComponent<RadarTargetComputer>();
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
    }

    void Start()
    {
        HitList = PulseSender.InstantiateRadarBlips(blipCount, blipTimeOut);
    }

    public void FixedUpdate()
    {
        if (!radarOn)
            return;
        xSweepRotationAngle -= Time.fixedDeltaTime * SweepSpeed;
        transform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        var hits = PulseSender.SendAndRecieveRadarPulse(HitList);
        if (hits.Length == 0)
            return;
        foreach (var hit in hits.Select(hit => hit.transform).ToArray())
            AddRadarHit(hit);
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
