using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmitterMemory : MonoBehaviour
{
    public GameObject Memory;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Memory.transform);
    }
}
