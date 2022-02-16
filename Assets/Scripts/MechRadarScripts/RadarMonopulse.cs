using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadarMonopulse : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    [SerializeField] public LayerMask RadarLayer;
    float sideAngleAdjustDegree = 1f;
    const float sideAngleAdjustDegreeAdjust = 0.01f;
    const float sideAngleAdjustDegreeMax = 2f;
    const float sideAngleAdjustDegreeLowest = 0.1f;
    private Collider localeCollider;
    public RaycastHit[] LobeHitsLeft = null;
    public RaycastHit[] LobeHitsRight = null;
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
        LobeHitsLeft = SendAndRecieveRadarPulse();
        RotateTransform(true);

        RotateTransform(true);
        LobeHitsRight = SendAndRecieveRadarPulse();
        RotateTransform(false);

        if (LobeHitsLeft.Length > LobeHitsRight.Length)
            rightLeftBalance = 0;
        else if (LobeHitsRight.Length > LobeHitsLeft.Length)
            rightLeftBalance = 1;
        else if (Mathf.Abs(LobeHitsRight.Length - LobeHitsLeft.Length) < 4)
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

        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(q.eulerAngles.x, q.eulerAngles.y, 0);
        transform.rotation = q;
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

    private RaycastHit[] SendAndRecieveRadarPulse()
    {
        var lobeHits = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, 500, RadarLayer);
        if (lobeHits.Length < 1)
            return null;
        foreach (var hit in lobeHits)
        {
            if(hit.distance != 0)
            {
                var radarHit = Instantiate(RadarBlip, hit.point, new Quaternion());
                radarHit.gameObject.GetComponent<RadarBlipScript>().DisappearTimerMax = 0.5f;
                radarHit.gameObject.GetComponent<Renderer>().materials[1].color = leftToRight ? Color.blue : Color.yellow;
            }
        }
        return lobeHits.Where(hit => hit.distance > 5).ToArray();
    }

    private Transform CreateRadarBlip() => Instantiate(RadarBlip, transform.position + Vector3.down * 3, new Quaternion());
}
