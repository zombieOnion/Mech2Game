using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShootAtPlayer : MonoBehaviour
{
    public GameObject PlayerMech;
    private MechShoot mechShoot;
    public float ShootReloadTime = 1f;
    float currentReloadTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMech = GameObject.FindGameObjectsWithTag("Player")[0];
        mechShoot = gameObject.GetComponent<MechShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentReloadTime > ShootReloadTime)
        {
            var randomShootDispersionFactor = Random.Range(1, 10);
            gameObject.transform.LookAt(PlayerMech.transform.position+new Vector3(randomShootDispersionFactor, randomShootDispersionFactor,0));
            mechShoot.OnFire1();
            currentReloadTime = 0f;
        }
        else
        {
            currentReloadTime += Time.deltaTime;
        }
    }
}
