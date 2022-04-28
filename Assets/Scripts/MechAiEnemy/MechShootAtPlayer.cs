using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShootAtPlayer : MonoBehaviour
{
    public GameObject PlayerMech;
    private MechShoot mechShoot;
    private RadarTrackerScript radarTargetComputerScript;
    public float ShootReloadTime = 1f;
    float currentReloadTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMech = GameObject.FindGameObjectsWithTag("Player")[0];
        mechShoot = gameObject.GetComponent<MechShoot>();
        radarTargetComputerScript = gameObject.transform.parent.gameObject.GetComponentInChildren<RadarTrackerScript>();
        PlayerMech.GetComponentInChildren<JammerScript>().JammedEnemy += FireGuidedMissile;
    }

    // Update is called once per frame
    void Update()
    {
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
        mechShoot.OnChangeWeapon();
        mechShoot.LockTarget(radarTargetComputerScript.CurrentlyTrackedTarget.transform);
        mechShoot.FireMainGun();
        mechShoot.OnChangeWeapon();
    }
}
