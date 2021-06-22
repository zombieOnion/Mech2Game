using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSweepScript : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    public Transform RadarSweep;
    public float rotationSpeed;
    public float radarDistance;
    [SerializeField] public LayerMask RadarLayer;
    private Collider RadarSweepCollider;

    // Start is called before the first frame update
    void Awake()
    {
        RadarSweep = transform.Find("RadarSweep");
        RadarSweepCollider = RadarSweep.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        RadarSweep.eulerAngles -= new Vector3(0, rotationSpeed * Time.deltaTime, 0);
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(RadarSweepCollider.bounds.center, RadarSweep.localScale, RadarSweep.forward, RadarSweep.rotation, 500, RadarLayer);
        int index = 0;
        foreach(RaycastHit hit in hits) {
            if(hits.Length > 0 && hit.distance != 0) {
                Instantiate(RadarBlip, hit.point, new Quaternion());
            }
            index += 1;
        }
    }
}
