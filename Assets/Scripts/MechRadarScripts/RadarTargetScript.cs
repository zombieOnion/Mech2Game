using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarTargetScript : MonoBehaviour
{
    public bool StayActive = false;
    public Transform TargetTransform = null;
    private float DisappearTimer;
    public float DisappearTimerMax;

    // Update is called once per frame
    void Update() {
        if(StayActive == false && TargetTransform == null) {
            DisappearTimer += Time.deltaTime;
            if(DisappearTimer >= DisappearTimerMax)
                Destroy(gameObject);
        }
        else {
            DisappearTimer = 0f;
        }
    }

    public void ReceiveNewRadarHitOnTarget(Transform newHit) { }
}
