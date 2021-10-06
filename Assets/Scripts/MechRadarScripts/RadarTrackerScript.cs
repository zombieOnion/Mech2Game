using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTrackerScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public bool TrackingTarget = false;
    public RadarTargetScript CurrentlyTrackedTarget = null;
    [SerializeField] public LayerMask RadarLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TrackingTarget == false)
            return;
        RaycastHit[] leftLobe;
        leftLobe = Physics.BoxCastAll(transform.position, transform.localScale, new Vector3(transform.forward.x-1f, transform.forward.y, transform.forward.z), transform.rotation, 500, RadarLayer);
        int index = 0;
        foreach(RaycastHit hit in leftLobe) {
            if(leftLobe.Length > 0 && hit.distance != 0) {
                var radarHit = Instantiate(RadarBlip, hit.point, new Quaternion());
                radarHit.gameObject.GetComponent<Renderer>().materials[1].color = Color.blue;
            }
            index += 1;
        }

    }

    public void TrackTarget(RadarTargetScript target) {
        TrackingTarget = true;
        CurrentlyTrackedTarget = target;
        transform.LookAt(target.transform);
    }

}
