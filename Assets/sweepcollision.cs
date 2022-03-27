using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweepcollision : MonoBehaviour
{
    [SerializeField] public Transform RadarBlip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(RadarBlip, collision.GetContact(0).point, new Quaternion());
    }
    private void OnTriggerEnter(Collider collision)
    {
        Instantiate(RadarBlip, collision.transform.position, new Quaternion());
    }


}
