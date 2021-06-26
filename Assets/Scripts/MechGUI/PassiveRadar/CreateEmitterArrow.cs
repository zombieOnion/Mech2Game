using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public GameObject EmitterManagerList;

public class CreateEmitterArrow : MonoBehaviour
{
    public List<GameObject> EmittersFC = new List<GameObject>();
    public GameObject arrow;

    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        //check the scene for Emitter objects and place them in list        
        EmittersFC = GameObject.FindGameObjectsWithTag("EmitterFC").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //create arrows for every emitter in the scene
        for (int i = 0; i <= EmittersFC.Count; i++)
        {
            
            //object is arrow
            //create dummy object, rotate it to LookAt at EmittersFC[i].transform and copy it's transform
            Instantiate(arrow);
            arrow.transform.LookAt(EmittersFC[i].transform);
        }
        EmittersFC.Clear();       
        
    }

}
