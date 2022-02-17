using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShootAtPlayer : MonoBehaviour
{
    public GameObject PlayerMech;
    private MechShoot mechShoot;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMech = GameObject.FindGameObjectsWithTag("Player")[0];
        mechShoot = gameObject.GetComponent<MechShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(PlayerMech.transform.position);
        mechShoot.OnFire1();
    }
}
