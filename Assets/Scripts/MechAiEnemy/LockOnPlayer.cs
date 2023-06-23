using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LockOnPlayer : NetworkBehaviour
{
    public readonly Guid MechRadarComputerSignature = Guid.NewGuid();
    [SerializeField] GameObject PlayerMech;
    RadarTrackerScript radarTrackerScript;
    RadarTargetComputer radarTargetComputerScript;
    private bool hasInit = false;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        //tryInit();
        base.OnNetworkSpawn();
    }

    public void Update()
    {
        //if (!IsServer || hasInit == true) return;
        //tryInit();
    }

    public void Init(GameObject playerMech)
    {
        if (hasInit) return;
        radarTrackerScript = GetComponent<RadarTrackerScript>();
        radarTargetComputerScript = gameObject.transform.parent.gameObject.GetComponentInChildren<RadarTargetComputer>();
        radarTrackerScript.MechRadarComputerSignature = MechRadarComputerSignature;
        radarTargetComputerScript.MechRadarComputerSignature = MechRadarComputerSignature;
        PlayerMech = playerMech;
        ReEngagePlayer();
        hasInit = true;
    }

    public void ReEngagePlayer()
    {
        gameObject.transform.LookAt(PlayerMech.transform.position);
        if (radarTrackerScript.CurrentlyTrackedTarget)
            radarTargetComputerScript.DestroyTarget(radarTrackerScript.CurrentlyTrackedTarget.transform);
        var playerTarget = radarTargetComputerScript.CreateNewTarget(PlayerMech.transform.position, 0, true);
        playerTarget.GetComponent<MeshRenderer>().enabled = true;
        radarTargetComputerScript.TrackTarget(playerTarget.GetComponent<RadarTargetScript>());
    }

}
