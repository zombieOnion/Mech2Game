using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class RadarMonopulse : NetworkBehaviour
{
    // General variables
    public bool leftToRight = true;
    public Transform[] LobeHitsLeft = null;
    public Transform[] LobeHitsRight = null;
    public Transform[] LobeStraightAhead= null;
    public RadarHitList<Transform> HitListLeftLobe;
    public RadarHitList<Transform> HitListRightLobe;
    public RadarHitList<Transform> HitListStraightAhead;
    // 
    [SerializeField] float sideAngleAdjustDegree = 0.2f;
    const float sideAngleAdjustDegreeAdjust = 0.2f;
    const float sideAngleAdjustDegreeMax = 3f;
    const float sideAngleAdjustDegreeLowest = 0.2f;
    int targetDrift = 0; // -1 left, 1 right
    float driftTime = 0;
    public float driftMaxTime = 0.8f;
    int rightLeftBalance = 0; //0 only left hit, 1 only right hit, 2 both hit, 3 both null

    // Blip settings
    public SendRadarPulseAndCreateRadarEchoes PulseSender;
    public float BlipTimeOut = 0.5f;
    public int BlipSize = 20;
    public readonly Guid RadarSignature = Guid.NewGuid();

    void Awake()
    {
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        HitListLeftLobe = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, RadarSignature, SetColourOfBlip);
        HitListRightLobe = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, RadarSignature, SetColourOfBlip);
        HitListStraightAhead = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, RadarSignature, SetColourOfBlip);
        base.OnNetworkSpawn();
    }


    public void SearchAndTrack()
    {
        // SCAN SEGMENT
        LobeStraightAhead = PulseSender.SendAndRecieveRadarPulse(HitListLeftLobe);
        var lastRightLeftBalance = rightLeftBalance;
        RotateTransform(false);
        LobeHitsLeft = PulseSender.SendAndRecieveRadarPulse(HitListLeftLobe);
        RotateTransform(true);

        RotateTransform(true);
        LobeHitsRight = PulseSender.SendAndRecieveRadarPulse(HitListRightLobe);
        RotateTransform(false);

        if (LobeHitsLeft.Length > LobeHitsRight.Length)
            rightLeftBalance = 0;
        else if (LobeHitsRight.Length > LobeHitsLeft.Length)
            rightLeftBalance = 1;
        else if (Mathf.Abs(LobeHitsRight.Length - LobeHitsLeft.Length) < 4 && (LobeHitsRight.Length > 0 && LobeHitsLeft.Length > 0))
            rightLeftBalance = 2;
        else
            rightLeftBalance = 3;
        //Debug.Log($"left and right {rightLeftBalance} leftToRight {leftToRight.ToString()} last {lastRightLeftBalance}");

        /*
        if (rightLeftBalance == 0)
        {
            RotateTransform(false);
            targetDrift = -1;
        }
        else if (rightLeftBalance == 1)
        {
            RotateTransform(true);
            targetDrift = 1;
        }
        
        if (rightLeftBalance != 2 && sideAngleAdjustDegree < sideAngleAdjustDegreeMax)
            sideAngleAdjustDegree += sideAngleAdjustDegreeAdjust;
        else if (rightLeftBalance == 2 && sideAngleAdjustDegree > sideAngleAdjustDegreeLowest)
            sideAngleAdjustDegree -= sideAngleAdjustDegreeAdjust;
        */

        // ADJUST SEGMENT BASED ON SCAN
        //0 only left hit, 1 only right hit, 2 both hit, 3 both null
        if(LobeStraightAhead.Length > 0 )
        {
            //Debug.Log($"straight ahead leftToRight {leftToRight.ToString()}");
            DecreaseSearchAngle();
            DecreaseSearchAngle();
        }
        if((rightLeftBalance == 0 && lastRightLeftBalance == 0) || (lastRightLeftBalance == 3 && rightLeftBalance == 0))
        {
            IncreaseSearchAngle();
            RotateTransform(false);
        }
        else if ((rightLeftBalance == 2 && lastRightLeftBalance == 0) || (lastRightLeftBalance == 2 && rightLeftBalance == 0))
        {
            DecreaseSearchAngle();
            targetDrift = -1;
            RotateTransform(false);
        }
        else if((rightLeftBalance == 1 && lastRightLeftBalance == 1) || (lastRightLeftBalance == 3 && rightLeftBalance == 1))
        {
            IncreaseSearchAngle();
            RotateTransform(true);
        }
        else if ((rightLeftBalance == 2 && lastRightLeftBalance == 1) || (lastRightLeftBalance == 2 && rightLeftBalance == 1))
        {
            DecreaseSearchAngle();
            targetDrift = 1;
            RotateTransform(true);
        }
        else if (rightLeftBalance != 2)
        {
            IncreaseSearchAngle();
        }

        
        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        transform.rotation = q;
        /*
        if (rightLeftBalance == 3 && driftTime < driftMaxTime && targetDrift != 0)
        {
            transform.Rotate(new Vector3(0, sideAngleAdjustDegree * targetDrift, 0));
            driftTime += Time.fixedDeltaTime;
        }
        else if (rightLeftBalance != 3)
            driftTime = 0f;
        */

    }

    public void DecreaseSearchAngle()
    {
        if (sideAngleAdjustDegree - sideAngleAdjustDegreeAdjust < sideAngleAdjustDegreeLowest)
            sideAngleAdjustDegree = sideAngleAdjustDegreeLowest;
        else
            sideAngleAdjustDegree -= sideAngleAdjustDegreeAdjust;
    }

    public void IncreaseSearchAngle()
    {
        if (sideAngleAdjustDegree + sideAngleAdjustDegreeAdjust > sideAngleAdjustDegreeMax)
            sideAngleAdjustDegree = sideAngleAdjustDegreeMax;
        else
            sideAngleAdjustDegree += sideAngleAdjustDegreeAdjust;
    }

    private void RotateTransform(bool increase)
    {
        if(leftToRight)
        {
            if(increase)
                transform.Rotate(new Vector3(0, sideAngleAdjustDegree, 0));
            else
                transform.Rotate(new Vector3(0, -sideAngleAdjustDegree, 0));
        }
        else
        {
            if (increase)
                transform.Rotate(new Vector3(sideAngleAdjustDegree, 0, 0));
            else
                transform.Rotate(new Vector3(-sideAngleAdjustDegree, 0, 0));
        }
    }

    private void SetColourOfBlip(RadarBlipScript blipScript) => blipScript.gameObject.GetComponent<Renderer>().materials[0].color = leftToRight? Color.blue : Color.yellow;

}
