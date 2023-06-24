using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class SendRadarPulseAndCreateRadarEchoes : NetworkBehaviour
{
    [SerializeField] public Transform RadarBlip;
    [SerializeField] public LayerMask RadarLayer;
    public int RadarRange = 1000;
    private Collider localeCollider;

    // Start is called before the first frame update
    void Awake()
    {
        localeCollider = gameObject.GetComponent<Collider>();
    }

    public RadarHitList<Transform> InstantiateRadarBlips(int size, float disappearTime, Guid signature, Action<RadarBlipScript> modifyBlip = null)
    {
        return InstantiateRadarBlipsGeneral(size, disappearTime, transform.position + Vector3.down * 5, signature, RadarBlip, modifyBlip);
    }

    public static RadarHitList<Transform> InstantiateRadarBlipsGeneral(int size, float disappearTime, Vector3 pos, Guid signature, Transform preFab, Action < RadarBlipScript> modifyBlip = null )
    {
        var lobeHits = new RadarHitList<Transform>(size);
        for (int i = 0; i < size; i++)
        {
            var radarHit = Instantiate(preFab, pos, new Quaternion());
            var blipScript = radarHit.gameObject.GetComponent<RadarBlipScript>();
            blipScript.radarSignature = signature;
            if (modifyBlip != null)
                modifyBlip(blipScript);
            lobeHits.Add(radarHit);
            radarHit.GetComponent<NetworkObject>().Spawn();
            blipScript.DisappearTimerMax.Value = disappearTime;
        }
        return lobeHits;
    }

    public Transform[] SendAndRecieveRadarPulse(RadarHitList<Transform> blipPool, Action<RadarBlipScript, RaycastHit> modifyBlip = null)
    {
        var lobeHits = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, RadarRange, RadarLayer);
        if (lobeHits.Length < 1)
            return new Transform[0];
        var blipHits = new List<Transform>();
        foreach (var hit in lobeHits)
        {
            if (hit.distance != 0)
            {
                var nextHit = blipPool.AdvanceNext();
                var nextHitScript = nextHit.GetComponent<RadarBlipScript>();
                nextHitScript.ResetAppearTime();
                nextHitScript.ResetAppearTimeClientRpc();
                if (modifyBlip != null)
                    modifyBlip(nextHitScript, hit);
                nextHit.position = hit.point;
                nextHit.rotation = Quaternion.identity;
                blipHits.Add(nextHit);
            }
        }
        return blipHits.ToArray();
        //return lobeHits.Where(hit => hit.distance > 5).ToArray();
    }
}
