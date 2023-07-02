using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DisappearTimerScript : NetworkBehaviour
{
    private float DisappearTimer = 0;
    public NetworkVariable<float> DisappearTimerMax;
    private bool hasStarted = false;
    private IEnumerator coroutine;
    public GameObject prefab;

    public override void OnNetworkSpawn()
    {
        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsServer)
        {
            coroutine = CheckDisappear();
            /*coroutine = CheckDisappear();
            if (DisappearTimerMax.Value == 0)
                DisappearTimerMax.Value = 2f;*/
        }
        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsServer)
        {
            StopCoroutine(coroutine);
        }
        base.OnNetworkDespawn();
    }
    /*private void Update()
    {
        DisappearTimer += Time.deltaTime;
        if (DisappearTimer >= DisappearTimerMax.Value)
        {
            Destroy(gameObject);
        }
    }*/

    public void StartCountDown()
    {
        if (hasStarted)
            return;
        hasStarted = true;
        StartCoroutine(coroutine);
    }

    IEnumerator CheckDisappear()
    {
        yield return new WaitForSeconds(DisappearTimerMax.Value);
        Destroy(gameObject);
    }

    private void EnableRenderer()
    {
        gameObject.GetComponent<EnableDisableRendererInterface>().EnableRenderer();
    }

    private void DisableRenderer()
    {
        gameObject.GetComponent<EnableDisableRendererInterface>().DisableRenderer();
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

