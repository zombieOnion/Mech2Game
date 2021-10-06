using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSweepScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    private Transform radarSweepTransform;
    private Transform mechTransform;
    public float rotationSpeed;
    public float radarDistance;
    [SerializeField] public LayerMask RadarLayer;
    private RadarTargetComputer targetProcessor;
    private Collider radarSweepCollider;
    private bool radarOn = true;
    public bool RadarOn { get => radarOn; set => radarOn = value; }
    private float xSweepRotationAngle;
    
    // Start is called before the first frame update
    void Awake()
    {
        radarSweepTransform = transform;
        radarSweepCollider = radarSweepTransform.GetComponent<Collider>();
        mechTransform = transform.parent.parent;
        targetProcessor = gameObject.GetComponent<RadarTargetComputer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!radarOn)
            return;
        //radarSweepTransform.eulerAngles -= new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        xSweepRotationAngle -= Time.deltaTime * rotationSpeed;
        radarSweepTransform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(radarSweepCollider.bounds.center, radarSweepTransform.localScale, radarSweepTransform.forward, radarSweepTransform.rotation, 500, RadarLayer);
        int index = 0;
        foreach(RaycastHit hit in hits) {
            if(hits.Length > 0 && hit.distance != 0) {
                targetProcessor.AddRadarHit(Instantiate(RadarBlip, hit.point, new Quaternion()).transform);
            }
            index += 1;
        } 
        
    }

    /*void FixedUpdate() {
        radarSweepTransform.transform.position = new Vector3(mechTransform.position.x, mechTransform.position.y+1, transform.position.z);
    }*/

    public void ChangeSweepDirection() {
        rotationSpeed *= -1;
    }

    public void SetScanSweepSpeedFull() {
        if(rotationSpeed > 0)
            rotationSpeed = 180;
        else
            rotationSpeed = -180;
    }

    public void SetScanSweepSpeedHalf() {
        if(rotationSpeed > 0)
            rotationSpeed = 90;
        else
            rotationSpeed = -90;
    }

    private void OnCollisionEnter(Collision collision) {
        targetProcessor.AddRadarHit(Instantiate(RadarBlip, collision.GetContact(0).point, new Quaternion()).transform);
    }

    private void OnTriggerEnter(Collider other) {
        targetProcessor.AddRadarHit(Instantiate(RadarBlip, other.transform.position, new Quaternion()).transform);

    }
}
