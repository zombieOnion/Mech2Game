using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnPlayer : MonoBehaviour
{
    public readonly Guid MechRadarComputerSignature = new Guid();
    [SerializeField] GameObject PlayerMech;
    RadarTrackerScript radarTrackerScript;
    RadarTargetComputer radarTargetComputerScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMech = GameObject.FindGameObjectsWithTag("Player")[0];
        radarTrackerScript = GetComponent<RadarTrackerScript>();
        radarTargetComputerScript = gameObject.transform.parent.gameObject.GetComponentInChildren<RadarTargetComputer>();
        radarTrackerScript.MechRadarComputerSignature = MechRadarComputerSignature;
        radarTargetComputerScript.MechRadarComputerSignature = MechRadarComputerSignature;
        var playerTarget = radarTargetComputerScript.CreateNewTarget(PlayerMech.transform.position);
        radarTargetComputerScript.TrackTarget(playerTarget);
    }
}
