using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnPlayer : MonoBehaviour
{
    public readonly Guid MechRadarComputerSignature = Guid.NewGuid();
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
        gameObject.transform.LookAt(PlayerMech.transform.position);
        /*var playerTarget = radarTargetComputerScript.CreateNewTarget(PlayerMech.transform.position);
        playerTarget.GetComponent<MeshRenderer>().enabled = false;
        radarTargetComputerScript.TrackTarget(playerTarget);*/
    }

}
