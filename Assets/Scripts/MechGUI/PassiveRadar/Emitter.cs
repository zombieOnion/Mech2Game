using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Emitter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject EmitterMemory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(EmitterMemory.transform);
    }
}
