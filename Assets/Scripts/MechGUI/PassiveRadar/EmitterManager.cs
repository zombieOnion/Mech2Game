using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EmitterManager : MonoBehaviour
{

   public List<GameObject> EmittersFC = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //check the scene for Emitter objects and place them in list        
        EmittersFC = GameObject.FindGameObjectsWithTag("EmitterFC").ToList();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
