using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MechShootAtPlayer : NetworkBehaviour
{
    public GameObject PlayerMech;
    private MechShoot mechShoot;
    private RadarTrackerScript radarTargetComputerScript;
    public float ShootReloadTime = 1f;
    float currentReloadTime = 0f;
    bool hasInit = false;
    private bool playerSpawningIsDone = false;

    // Start is called before the first frame update
    void Awake()
    {
        mechShoot = gameObject.GetComponent<MechShoot>();
        radarTargetComputerScript = gameObject.transform.parent.gameObject.GetComponentInChildren<RadarTrackerScript>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            base.OnNetworkDespawn();
            return;
        }
        SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged += OnAllPlayerSpawnedValueChanged;
        base.OnNetworkDespawn();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged -= OnAllPlayerSpawnedValueChanged;
            base.OnNetworkDespawn();
            return;
        }
        base.OnNetworkDespawn();
    }

    private void OnAllPlayerSpawnedValueChanged(bool oldValue, bool newValue) => playerSpawningIsDone = newValue;

    private void tryInitShootPlayer()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0 && hasInit == false && playerSpawningIsDone)
            initShootPlayer();
    }

    private void initShootPlayer()
    {
        PlayerMech = GameObject.FindGameObjectsWithTag("Player")[0];
        if(radarTargetComputerScript)
            transform.root.GetComponentInChildren<LockOnPlayer>().Init(PlayerMech);
        PlayerMech.GetComponentInChildren<JammerScript>().JammedEnemy += FireGuidedMissile;
        hasInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        if (hasInit == false)
            tryInitShootPlayer();
        if (hasInit == false)
            return;
        if (currentReloadTime > ShootReloadTime)
        {
            var randomShootDispersionFactorX = UnityEngine.Random.Range(-5, 5);
            var randomShootDispersionFactorY = UnityEngine.Random.Range(0, 10);
            gameObject.transform.LookAt(PlayerMech.transform.position+new Vector3(randomShootDispersionFactorX, randomShootDispersionFactorY, 0));
            mechShoot.OnFire1();
            currentReloadTime = 0f;
        }
        else
        {
            currentReloadTime += Time.deltaTime;
        }
    }


    void FireGuidedMissile(object sender, EventArgs e)
    {
        if(radarTargetComputerScript == null) return;
        transform.root.GetComponentInChildren<LockOnPlayer>().ReEngagePlayer();
        mechShoot.OnChangeWeapon();
        gameObject.transform.LookAt(PlayerMech.transform.position);
        mechShoot.LockTarget(radarTargetComputerScript.CurrentlyTrackedTarget.transform);
        mechShoot.FireMainGunServerRpc();
        mechShoot.OnChangeWeapon();
    }
}
