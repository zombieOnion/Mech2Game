using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisappearTimerLocaleScript : MonoBehaviour
{
    public float DisappearTimerMax;
    private Coroutine coroutine;
    private BoxCollider boxCollider;
    private EnableDisableRendererInterface rendererInterface;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        rendererInterface = gameObject.GetComponent<EnableDisableRendererInterface>();
    }

    void Start()
    {
        StartCountDown();
    }

    void OnDestroy()
    {
        StopCoroutine(coroutine);
    }

    public void StartCountDown()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(CheckDisappear());
    }

    private IEnumerator CheckDisappear()
    {
        EnableRenderer();
        yield return new WaitForSeconds(DisappearTimerMax);
        DisableRenderer();
    }

    private void EnableRenderer()
    {
        if(boxCollider != null)
        {
            boxCollider.enabled = true;
        }
        rendererInterface.EnableRenderer();
    }

    private void DisableRenderer()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        rendererInterface.DisableRenderer();
    }


    public static RadarHitList<Transform> InstantiatePrefabWithDisappearsGeneralLocale(int size, float disappearTime, Vector3 pos, Transform preFab)
    {
        var lobeHits = new RadarHitList<Transform>(size);
        for (int i = 0; i < size; i++)
        {
            var radarHit = Instantiate(preFab, pos, new Quaternion());
            var blipScript = radarHit.gameObject.GetComponent<DisappearTimerLocaleScript>();
            lobeHits.Add(radarHit);
            blipScript.DisappearTimerMax = disappearTime;
        }
        return lobeHits;
    }

    public static Transform CreateLocaleRadarBlip(RadarHitList<Transform> HitList, Vector3 dir)
    {
        var nextHit = HitList.AdvanceNext();
        var rendererControlScript = nextHit.GetComponent<DisappearTimerLocaleScript>();
        rendererControlScript.DisappearTimerMax = 2;
        nextHit.position = dir;
        rendererControlScript.StartCountDown();
        return nextHit;
    }
}

