using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float thrust;
    public Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * thrust, ForceMode.VelocityChange);
    }
}
