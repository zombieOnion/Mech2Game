using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSweepScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public RadarHitList<Transform> HitList;
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

    void Start()
    {
        HitList = new RadarHitList<Transform>(300);
        for (int i = 0; i < 300; i++)
        {
            var radarblip = Instantiate(RadarBlip, transform.position + Vector3.down*5, new Quaternion());
            HitList.Add(radarblip);
        }
    }

    void Update()
    {
        if(!radarOn)
            return;
        //radarSweepTransform.eulerAngles -= new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        xSweepRotationAngle -= Time.deltaTime * rotationSpeed;
        radarSweepTransform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(radarSweepCollider.bounds.center, radarSweepTransform.localScale, radarSweepTransform.forward, radarSweepTransform.rotation, 500, RadarLayer);
        foreach(RaycastHit hit in hits) {
           if(hit.distance > 5) {
                var nextHit = HitList.AdvanceNext();
                var nextHitScript = nextHit.GetComponent<RadarBlipScript>();
                nextHitScript.gameObject.SetActive(true);
                nextHitScript.ResetAppearTime();
                nextHit.position = hit.point;
                nextHit.rotation = Quaternion.identity;
                targetProcessor.AddRadarHit(nextHit);
           }
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
}
