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
    private NetworkObjectPoolSpawner spawner;

    // Start is called before the first frame update
    void Awake()
    {
        localeCollider = gameObject.GetComponent<Collider>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        spawner = FindAnyObjectByType<NetworkObjectPoolSpawner>();
        base.OnNetworkSpawn();
    }

    public RadarHitList<Transform> InstantiateRadarBlips(int size, float disappearTime, Guid signature, ulong id, Action<RadarBlipScript> modifyBlip = null)
    {
        return InstantiateRadarBlipsGeneral(size, disappearTime, transform.position + Vector3.down * 5, signature, RadarBlip, id, modifyBlip);
    }

    public static RadarHitList<Transform> InstantiateRadarBlipsGeneral(int size, float disappearTime, Vector3 pos, Guid signature, Transform preFab, ulong id, Action < RadarBlipScript> modifyBlip = null )
    {
        var lobeHits = DisappearTimerScript.InstantiateRadarBlipsGeneral(size, disappearTime, pos, preFab);
        foreach (var radarHit in lobeHits.GetLast(size))
        {
            var blipScript = radarHit.gameObject.GetComponent<RadarBlipScript>();
            blipScript.CreatorId = id;
            blipScript.radarSignature = signature;
            if (modifyBlip != null)
                modifyBlip(blipScript);
            lobeHits.Add(radarHit);
        }
        return lobeHits;
    }

    public Transform[] SendAndRecieveRadarPulse(RadarHitList<Transform> hitList = null, Action<RadarBlipScript, RaycastHit> modifyBlip = null)
    {
        var lobeHits = Physics.BoxCastAll(localeCollider.bounds.center, transform.localScale, transform.forward, transform.rotation, RadarRange, RadarLayer);
        if (lobeHits.Length < 1)
            return new Transform[0];
        var blipHits = new List<Transform>();
        foreach (var hit in lobeHits)
        {
            if (hit.distance != 0)
            {
                Transform nextHit;
                if (hitList == null)
                {
                    nextHit = spawner.SpawnRadarBlip(hit.point, Quaternion.identity).transform; //blipPool.AdvanceNext();
                    nextHit.GetComponent<NetworkObject>().Spawn(destroyWithScene: true);
                    var rendererControlScript = nextHit.GetComponent<DisappearTimerScript>();
                    rendererControlScript.DisappearTimerMax.Value = 2;
                    rendererControlScript.StartCountDown();
                }
                else
                {
                    nextHit = DisappearTimerLocaleScript.CreateLocaleRadarBlip(hitList, hit.point);
                }
                //rendererControlScript.ResetAppearTimer();
                //rendererControlScript.ResetAppearTimerClientRpc();
                var nextHitScript = nextHit.GetComponent<RadarBlipScript>();
                if (modifyBlip != null)
                    modifyBlip(nextHitScript, hit);
                blipHits.Add(nextHit.transform);
            }
        }
        return blipHits.ToArray();
        //return lobeHits.Where(hit => hit.distance > 5).ToArray();
    }

    public static void SerParentList(RadarHitList<Transform> HitList, Transform parent)
    {
        foreach (var hit in HitList.GetLast(HitList.Size))
        {
            hit.GetComponent<NetworkObject>().TrySetParent(parent.root, true);
        }
    }
}
