using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RadarSweepScriptAiEnemySubclass : RadarSweepScript
{
    private Action<RadarBlipScript> hideAiBlips => (blip =>
    {
        //blip.GetComponent<Renderer>().enabled = false;
        blip.gameObject.GetComponent<Renderer>().materials[0].color = Color.magenta;
    });
    private Action<RadarBlipScript, RaycastHit> onlyBlipOnPlayer => ((blip, hit) =>
    {
        //blip.GetComponent<Renderer>().enabled = false;
        if(hit.transform.root.gameObject.tag != "Player")
        {
            var blipScript = blip.GetComponent<DisappearTimerLocaleScript>();
            blipScript.DisappearTimerMax = 0.01f;
            return;
        }
        blip.gameObject.GetComponent<Renderer>().materials[0].color = Color.magenta;
    });

    protected override void CreateTargetCache()
    {
        var currentPos = transform.position;
        HitList = DisappearTimerLocaleScript.InstantiatePrefabWithDisappearsGeneralLocale(blipCount, blipTimeOut, new Vector3(currentPos.x, currentPos.y - 10, currentPos.z), base.RadarBlipLocalePreFab.transform);
        HitList.GetLast(HitList.Size).ForEach(h => hideAiBlips(h.GetComponent<RadarBlipScript>()));
    }

    protected override Transform[] SendAndCreateTargets()
    {
        return PulseSender.SendAndRecieveRadarPulse(HitList, modifyBlip: onlyBlipOnPlayer);
    }
}
