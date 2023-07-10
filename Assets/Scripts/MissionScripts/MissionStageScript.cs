using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MissionStageScript2 : NetworkBehaviour
{
    private string missionText;

    // Start is called before the first frame update
    public string MissionText { get => missionText; private set => missionText = value; }
    [SerializeField]
    public GoSpawnInfo[] NgosToSpawn;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

