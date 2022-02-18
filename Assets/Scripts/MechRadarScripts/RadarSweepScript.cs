using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSweepScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public RadarHitList<Transform> HitList;
    private Transform radarSweepTransform;
    private Transform mechTransform;
    public float radarDistance;
    [SerializeField] public LayerMask RadarLayer;
    private RadarTargetComputer targetProcessor;
    private Collider radarSweepCollider;
    private bool radarOn = true;
    private int blipCount = 1000;
    private float blipTimeOut = 5;
    public float SweepSpeed;
    public float SweepSpeedChangeNumber = 60f;
    public int MaxSweepSpeed = 360;
    public int MinimumSweepSpeed = 30;
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
        HitList = new RadarHitList<Transform>(blipCount);
        for (int i = 0; i < blipCount; i++)
        {
            var radarblip = Instantiate(RadarBlip, transform.position + Vector3.down*5, new Quaternion());
            radarblip.GetComponent<RadarBlipScript>().DisappearTimerMax = blipTimeOut;
            HitList.Add(radarblip);
        }
    }

    void FixedUpdate()
    {
        if(!radarOn)
            return;
        //radarSweepTransform.eulerAngles -= new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        xSweepRotationAngle -= Time.fixedDeltaTime * SweepSpeed;
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
