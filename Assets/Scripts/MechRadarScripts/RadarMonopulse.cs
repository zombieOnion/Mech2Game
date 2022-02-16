using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarMonopulse : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    [SerializeField] public LayerMask RadarLayer;
    float sideAngleAdjustDegree = 1f;
    const float sideAngleAdjustDegreeAdjust = 0.01f;
    const float sideAngleAdjustDegreeMax = 5f;
    const float sideAngleAdjustDegreeLowest = 0.3f;
    private Collider localeCollider;
    public RaycastHit? LobeHitLeft = null;
    public RaycastHit? LobeHitRight = null;
    int targetDrift = 0; // -1 left, 1 right
    int rightLeftBalance = 0; //0 only left hit, 1 only right hit, 2 both hit, 3 both null
    private Transform RadarBlipLeftLobe;
    private Transform RadarBlipRightLobe;

    public bool leftToRight = true;

    void Start()
    {
        localeCollider = gameObject.GetComponent<Collider>();
        var RadarBlipLeftLobe = CreateRadarBlip();
        var RadarBlipRightLobe = CreateRadarBlip();
    }

    public void SendMonoPulse()
    {
        RotateTransform(false);
        LobeHitLeft = SendAndRecieveRadarPulse(ref RadarBlipLeftLobe);
        RotateTransform(true);

        RotateTransform(true);
        LobeHitRight = SendAndRecieveRadarPulse(ref RadarBlipRightLobe);
        RotateTransform(false);

        if (LobeHitLeft == null && LobeHitRight == null)
            rightLeftBalance = 3;
        else if (LobeHitLeft != null && LobeHitRight == null)
            rightLeftBalance = 0;
        else if (LobeHitLeft == null && LobeHitRight != null)
            rightLeftBalance = 1;
        else
            rightLeftBalance = 2;

        Debug.Log($"left {(LobeHitLeft == null ? "0" : "1")} right {(LobeHitRight == null ? "0" : "1" )} ");
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

        //if (leftLobe.Length == 0 && rightLobe.Length == 0)
        //    transform.Rotate(new Vector3(0, +sideAngleAdjustDegree * targetDrift, 0));

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

    private RaycastHit? SendAndRecieveRadarPulse(ref Transform radarBlip)
    {
        var lobe = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, 500, RadarLayer);
        
        if (lobe.Length > 0 && lobe[0].distance != 0)
        {

            if(radarBlip == null)            
                radarBlip = Instantiate(RadarBlip, lobe[0].point, new Quaternion());
            else
                radarBlip.transform.position = lobe[0].point;

            var radarHit = radarBlip.transform;
            radarHit.gameObject.GetComponent<RadarBlipScript>().DisappearTimerMax = 4;
            radarHit.gameObject.GetComponent<Renderer>().materials[1].color = Color.blue;
            return lobe[0];
        }
        else
            return null;
        
    }

    private Transform CreateRadarBlip() => Instantiate(RadarBlip, transform.position + Vector3.down * 3, new Quaternion());
}
