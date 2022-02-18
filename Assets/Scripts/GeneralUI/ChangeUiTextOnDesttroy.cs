using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUiTextOnDesttroy : MonoBehaviour
{
    public GameObject[] DisableUiElements;
    public GameObject[] ActivateUiElements;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (GameObject go in DisableUiElements)
            go.SetActive(false);
        foreach (GameObject go2 in ActivateUiElements)
            go2.SetActive(true);
    }
}
