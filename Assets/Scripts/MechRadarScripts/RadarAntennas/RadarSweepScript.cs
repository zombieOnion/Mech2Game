using System;
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

    // radar rotation and speed
    private float xSweepRotationAngle;
    public float SweepSpeedChangeNumber = 30f;
    public int MaxSweepSpeed = 150;
    public int MinimumSweepSpeed = 30;

    // sector sweep and change direction
    public float SweepSpeed;
    public bool IsSectorSweeping { get; private set; } = false;
    private float xSectorSweepStart = 45f;
    private float xSectorSweepEnd = 90f;
    private float xSectorSweepAngle = 45f;

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
        var lastXAngle = xSweepRotationAngle;
        xSweepRotationAngle += (Time.fixedDeltaTime/2) * SweepSpeed;
        transform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        if(IsSectorSweeping)
        {
            if (xSweepRotationAngle > xSectorSweepEnd && SweepSpeed > 0)
            {
                xSweepRotationAngle = xSectorSweepEnd - 3f;
                ChangeSweepDirection();
            }
            else if (xSweepRotationAngle < xSectorSweepStart && SweepSpeed < 0)
            {
                xSweepRotationAngle = xSectorSweepStart + 3f;
                ChangeSweepDirection();
            }
        }
        if (lastXAngle < 360f && xSweepRotationAngle > 360f)
            xSweepRotationAngle = 0;
        else if(lastXAngle > 0 && xSweepRotationAngle < 0)
            xSweepRotationAngle = 360;

        var hits = PulseSender.SendAndRecieveRadarPulse(HitList);
        if (hits.Length == 0)
            return;
        foreach (var hit in hits.Select(hit => hit.transform).ToArray())
            AddRadarHit(hit);
    }

    private void AddRadarHit(Transform radarHit)
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

    public void ChangeSweepDirection() {
        SweepSpeed *= -1;
    }

    public void IncreaseSweepSpeed() {
        if (Math.Abs(MaxSweepSpeed) <= Math.Abs(SweepSpeed))
            return;
        else
        {
            if(SweepSpeed > 0)
                SweepSpeed += SweepSpeedChangeNumber;
            else
                SweepSpeed -= SweepSpeedChangeNumber;
        }   
    }

    public void DecreaseSweepSpeed() {
        if (Math.Abs(MinimumSweepSpeed) >= Math.Abs(SweepSpeed))
            return;
        else
        {
            if (SweepSpeed > 0)
                SweepSpeed -= SweepSpeedChangeNumber;
            else
                SweepSpeed += SweepSpeedChangeNumber;
        }
    }

    public void ToggleSectorSweep()
    {
        IsSectorSweeping = !IsSectorSweeping;
        if(SweepSpeed < 0)
            SweepSpeed = Math.Abs(SweepSpeed);
    }
    public void IncreaseSectorSweep()
    {
        if(xSectorSweepEnd+xSectorSweepAngle > 360)
        {
            xSectorSweepStart = 0;
            xSectorSweepEnd = xSectorSweepAngle;
        }
        else
        {
            xSectorSweepStart += xSectorSweepAngle;
            xSectorSweepEnd += xSectorSweepAngle;
        }
        if (SweepSpeed < 0)
            SweepSpeed = SweepSpeed * -1;
    }
    public void DecreaseSectorSweep()
    {
        if (xSectorSweepStart-xSectorSweepAngle < 0)
        {
            xSectorSweepStart = 360-xSectorSweepAngle;
            xSectorSweepEnd = 360;
        }
        else
        {
            xSectorSweepStart -= xSectorSweepAngle;
            xSectorSweepEnd -= xSectorSweepAngle;
        }
        if (SweepSpeed > 0)
            SweepSpeed = SweepSpeed * -1;
    }
}
