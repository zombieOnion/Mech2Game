using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTargetProcessor : MonoBehaviour
{
    public RadarHitList HitList;
    public List<Transform> Targets = new List<Transform>();
    public int NumberDepthOfLastHits = 3;
    private float timeCounter = 0.0f;
    public float TimeBetweenProcessHits = 2.0f;
    [SerializeField] public Transform RadarTarget;
    private LayerMask plotLayermask = 1 << 6;
    private LayerMask targetLayermask = 1 << 8;
    // Start is called before the first frame update
    void Start()
    {
        HitList = new RadarHitList(300);
    }

    public void AddRadarHit(Transform hit) {
        HitList.Add(hit);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Transform radarHit = HitList.GetCurrent();
        if(radarHit==null)
            return;
                
        Collider[] overlapping = Physics.OverlapBox(radarHit.position, radarHit.localScale/2, Quaternion.identity, plotLayermask);
        if(overlapping.Length > 2) {
            var overlapingRadarTargets = Physics.OverlapBox(radarHit.position, radarHit.localScale / 2, Quaternion.identity, targetLayermask);
            bool alreadyMarked = false;
            foreach(var target in overlapingRadarTargets) {
                foreach(var alreadyPlottedTarget in Targets) {
                    if(target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID()) {
                        alreadyMarked = true;
                        break;
                    }
                }
                if(alreadyMarked)
                    break;
            }
            if(alreadyMarked==false)
                Targets.Add(Instantiate(RadarTarget, overlapping[0].transform.position, new Quaternion()).transform);
        }

        timeCounter = timeCounter + Time.deltaTime;
        if(timeCounter < TimeBetweenProcessHits)
            return;
        
    }
    /*
    private bool DoesTransformOverlapAny(Transform testTransform, IEnumerable<Transform> transformList) {

    } */
}
