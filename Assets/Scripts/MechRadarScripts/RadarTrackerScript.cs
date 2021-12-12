using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTrackerScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public bool TrackingTarget = false;
    public RadarTargetScript CurrentlyTrackedTarget = null;
    [SerializeField] public LayerMask RadarLayer;
    float sideAngleAdjustDegree = 2f;
    private Collider localeCollider;
    // Start is called before the first frame update
    void Start()
    {
        localeCollider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
        if(TrackingTarget == false)
            return;

        transform.Rotate(new Vector3(0, -sideAngleAdjustDegree - 2, 0));
        RaycastHit[] leftLobe = SendAndRecieveRadarPulse();
        transform.Rotate(new Vector3(0, +sideAngleAdjustDegree + 2, 0));

        RaycastHit[] rightLobe;
        transform.Rotate(new Vector3(0, +sideAngleAdjustDegree, 0));
        rightLobe = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, 500, RadarLayer);
        foreach(RaycastHit hit in rightLobe) {
            if(rightLobe.Length > 0 && hit.distance != 0) {
                var radarHit = Instantiate(RadarBlip, hit.point, new Quaternion());
                radarHit.gameObject.GetComponent<RadarBlipScript>().DisappearTimerMax = 10;
                radarHit.gameObject.GetComponent<Renderer>().materials[1].color = Color.blue;
            }
        }
        transform.Rotate(new Vector3(0, -sideAngleAdjustDegree, 0));
        Debug.Log($"left {leftLobe.Length} right {rightLobe.Length}");
        if(leftLobe.Length > rightLobe.Length)
            transform.Rotate(new Vector3(0, -sideAngleAdjustDegree, 0));
        else if(leftLobe.Length < rightLobe.Length)
            transform.Rotate(new Vector3(0, +sideAngleAdjustDegree, 0));

    }

    private RaycastHit[] SendAndRecieveRadarPulse() {
        var leftLobe = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, 500, RadarLayer);
        for(int i = 0; i < leftLobe.Length; i++) {
            RaycastHit hit = leftLobe[i];
            if(leftLobe.Length > 0 && hit.distance != 0) {
                var radarHit = Instantiate(RadarBlip, hit.point, new Quaternion());
                radarHit.gameObject.GetComponent<RadarBlipScript>().DisappearTimerMax = 10;
                radarHit.gameObject.GetComponent<Renderer>().materials[1].color = Color.blue;
            }
        }
        return leftLobe;
    }

    public void TrackTarget(RadarTargetScript target) {
        TrackingTarget = true;
        CurrentlyTrackedTarget = target;
        transform.LookAt(target.transform);

        

    }
}
