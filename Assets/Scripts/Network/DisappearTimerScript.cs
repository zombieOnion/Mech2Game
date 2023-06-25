using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DisappearTimerScript : NetworkBehaviour
{
    private float DisappearTimer;
    public NetworkVariable<float> DisappearTimerMax;
    private bool doneWithUpdate = false;
    private bool hasEnabled = false;
    
    void Update()
    {
        if (doneWithUpdate) return;
        DisappearTimer += Time.deltaTime;
        if (DisappearTimer >= DisappearTimerMax.Value && hasEnabled)
        {
            DisableRenderer();
            doneWithUpdate = true;
        }
        else if(!hasEnabled)
        {
            EnableRenderer();
            hasEnabled = true;
        }
    }

    private void EnableRenderer()
    {
        gameObject.GetComponent<EnableDisableRendererInterface>().EnableRenderer();
    }

    private void DisableRenderer()
    {
        gameObject.GetComponent<EnableDisableRendererInterface>().DisableRenderer();
    }

    public void ResetAppearTimer()
    {
        DisappearTimer = 0;
        doneWithUpdate = false;
        hasEnabled = false;
    }

    [ClientRpc]
    public void ResetAppearTimerClientRpc()
    {
        DisappearTimer = 0;
        doneWithUpdate = false;
        hasEnabled = false;
    }

    public static RadarHitList<Transform> InstantiateRadarBlipsGeneral(int size, float disappearTime, Vector3 pos, Transform preFab)
    {
        var lobeHits = new RadarHitList<Transform>(size);
        for (int i = 0; i < size; i++)
        {
            var radarHit = Instantiate(preFab, pos, new Quaternion());
            var blipScript = radarHit.gameObject.GetComponent<DisappearTimerScript>();
            lobeHits.Add(radarHit);
            radarHit.GetComponent<NetworkObject>().Spawn();
            blipScript.DisappearTimerMax.Value = disappearTime;
        }
        return lobeHits;
    }

}
