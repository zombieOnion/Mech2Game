using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public GameObject EmitterManagerList;

public class CreateEmitterArrow : MonoBehaviour
{
    public List<GameObject> EmittersFC = new List<GameObject>();
    public List<GameObject> RenderedEmittersFC = new List<GameObject>();
    public GameObject arrow;

    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        //check the scene for Emitter objects and place them in list   
        EmittersFC.Clear();
        EmittersFC = GameObject.FindGameObjectsWithTag("EmitterFC").ToList();
        RenderedEmittersFC.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //create arrows for every emitter in the scene
        //fill EmittersFC with gameobjects that are Emitters
        EmittersFC = GameObject.FindGameObjectsWithTag("EmitterFC").ToList();
        for (int i = 0; i <= EmittersFC.Count-1; i++)
        {
            if (RenderedEmittersFC.Contains(EmittersFC[i])==false) 
            {
                //add the emitter to the rendered emitters list:
                RenderedEmittersFC.Add(EmittersFC[i]);
                Instantiate(arrow, this.transform);
                arrow.transform.LookAt(EmittersFC[i].transform);
                //tell the arrow which emitter it is pointing towards
                arrow.GetComponent<EmitterMemory>().Memory = EmittersFC[i];
            }
            else
            {
                //do nothing
                //Instantiate(arrow);
            }
            
        }
        //empty both Emitters list and list of Emitters that have arrows assigned to them
        EmittersFC.Clear();
        RenderedEmittersFC.Clear();
    }

}
