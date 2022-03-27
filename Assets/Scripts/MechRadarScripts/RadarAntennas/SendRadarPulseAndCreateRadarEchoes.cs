using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SendRadarPulseAndCreateRadarEchoes : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    [SerializeField] public LayerMask RadarLayer;
    public int RadarRange = 1000;
    private Collider localeCollider;

    // Start is called before the first frame update
    void Start()
    {
        localeCollider = gameObject.GetComponent<Collider>();
    }

    public RadarHitList<Transform> InstantiateRadarBlips(int size, float disappearTime, Guid signature, Action<RadarBlipScript> modifyBlip = null)
    {
        var lobeHits = new RadarHitList<Transform>(size);
        for (int i = 0; i < size; i++)
        {
            var radarHit = Instantiate(RadarBlip, transform.position + Vector3.down * 5, new Quaternion());
            var blipScript = radarHit.gameObject.GetComponent<RadarBlipScript>();
            blipScript.DisappearTimerMax = disappearTime;
            blipScript.radarSignature = signature;
            if (modifyBlip != null)
                modifyBlip(blipScript);
            lobeHits.Add(radarHit);
            radarHit.gameObject.SetActive(false);
        }
        return lobeHits;
    }

    public RaycastHit[] SendAndRecieveRadarPulse(RadarHitList<Transform> blipPool, Action<RadarBlipScript, RaycastHit> modifyBlip = null)
    {
        var lobeHits = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, RadarRange, RadarLayer);
        if (lobeHits.Length < 1)
            return new RaycastHit[0];
        foreach (var hit in lobeHits)
        {
            if (hit.distance != 0)
            {
                var nextHit = blipPool.AdvanceNext();
                var nextHitScript = nextHit.GetComponent<RadarBlipScript>();
                nextHitScript.gameObject.SetActive(true);
                if (modifyBlip != null)
                    modifyBlip(nextHitScript, hit);
                nextHitScript.ResetAppearTime();
                nextHit.position = hit.point;
                nextHit.rotation = Quaternion.identity;
            }
        }
        return lobeHits.Where(hit => hit.distance > 5).ToArray();
    }
}
