using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadarTargetScript : MonoBehaviour
{
    public bool StayActive = true;
    public Transform TargetTransform = null;
    private int _radarHitModeStatus = 0; // 0 is waiting for new hits, 1 is actively receiving hits
    private float _maxTimeWaitForNewHit = 0.2f;
    private float _hitReceiverCountDown;
    private int _hitsReceivedInSweep = 0;

    private float DisappearTimer;
    public float DisappearTimerMax;
    public int MaxRadarHitHistory = 50;
    public RadarHitList<Transform> RadarHits;

    void Start() {
        RadarHits = new RadarHitList<Transform>(MaxRadarHitHistory);
        _hitReceiverCountDown = _maxTimeWaitForNewHit;
    }

    // Update is called once per frame
    void Update() {
        if(_radarHitModeStatus==1) {
            _hitReceiverCountDown += Time.deltaTime;
            if(_hitReceiverCountDown >= _maxTimeWaitForNewHit) {
                ProccessLatestHits();
            }
        }

        if(StayActive == false) {
            DisappearTimer += Time.deltaTime;
            if(DisappearTimer >= DisappearTimerMax)
                Destroy(gameObject);
        }
        else {
            DisappearTimer = 0f;
        }
    }

    private void ProccessLatestHits() {
        var thisSweepHits = RadarHits.GetLast(_hitsReceivedInSweep).Where(hit => hit != null).ToList();
        Bounds hitBounds = new Bounds(thisSweepHits[0].position, Vector3.zero);
        thisSweepHits.ForEach(hit => hitBounds.Encapsulate(hit.position));
        this.transform.position = new Vector3(hitBounds.center.x, Terrain.activeTerrain.SampleHeight(this.transform.position)+0.5f, hitBounds.center.z);
        _radarHitModeStatus = 0;
    }

    public void ReceiveNewRadarHitOnTarget(Transform newHit) {
        if(_radarHitModeStatus == 0) {
            _radarHitModeStatus = 1;
            _hitsReceivedInSweep = 0;
        }
        _hitReceiverCountDown = 0;
        RadarHits.Add(newHit);
        ++_hitsReceivedInSweep;
    }
}
