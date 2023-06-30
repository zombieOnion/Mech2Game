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
            var blipScript = blip.GetComponent<DisappearTimerScript>();
            blipScript.DisappearTimerMax.Value = 0;
            return;
        }
        blip.gameObject.GetComponent<Renderer>().materials[0].color = Color.magenta;
    });

    protected override void CreateTargetCache()
    {
        HitList = PulseSender.InstantiateRadarBlips(blipCount, blipTimeOut, RadarSignature, hideAiBlips);
        SendRadarPulseAndCreateRadarEchoes.SerParentList(HitList, gameObject.transform);
    }

    protected override Transform[] SendAndCreateTargets()
    {
        return PulseSender.SendAndRecieveRadarPulse(HitList, onlyBlipOnPlayer);
    }
}
