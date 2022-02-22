using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadarMonopulse : MonoBehaviour
{
    // General variables
    public bool leftToRight = true;
    public RaycastHit[] LobeHitsLeft = null;
    public RaycastHit[] LobeHitsRight = null;
    public RadarHitList<Transform> HitListLeftLobe;
    public RadarHitList<Transform> HitListRightLobe;

    // 
    float sideAngleAdjustDegree = 0.1f;
    const float sideAngleAdjustDegreeAdjust = 0.05f;
    const float sideAngleAdjustDegreeMax = 2f;
    const float sideAngleAdjustDegreeLowest = 0.01f;
    int targetDrift = 0; // -1 left, 1 right
    float driftTime = 0;
    public float driftMaxTime = 1;
    int rightLeftBalance = 0; //0 only left hit, 1 only right hit, 2 both hit, 3 both null

    // Blip settings
    public SendRadarPulseAndCreateRadarEchoes PulseSender;
    public float BlipTimeOut = 0.5f;
    public int BlipSize = 20;

    void Awake()
    {
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
    }

    void Start()
    {
        HitListLeftLobe = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, SetColourOfBlip);
        HitListRightLobe = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, SetColourOfBlip);
    }


    public void SearchAndTrack()
    {
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

        Debug.Log($"left and right {rightLeftBalance}");
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

        if(rightLeftBalance == 2 && lastRightLeftBalance == 0)
            RotateTransform(true);
        else if(rightLeftBalance == 2 && lastRightLeftBalance == 1)
            RotateTransform(false);


        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        transform.rotation = q;
        if (rightLeftBalance == 3 && driftTime < driftMaxTime)
        {
            transform.Rotate(new Vector3(0, sideAngleAdjustDegree * targetDrift, 0));
            driftTime += Time.fixedDeltaTime;
        }
        else if (rightLeftBalance != 3)
            driftTime = 0f;


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
