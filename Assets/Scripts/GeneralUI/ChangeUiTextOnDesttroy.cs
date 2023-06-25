using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChangeUiTextOnDesttroy : NetworkBehaviour
{
    public GameObject[] DisableUiElements;
    public GameObject[] ActivateUiElements;

    public override void OnDestroy()
    {
        if (DisableUiElements != null)
        {
            foreach (GameObject go in DisableUiElements)
            {
                go.SetActive(false);
                
            }
        }
        if (ActivateUiElements != null)
        {
            foreach (GameObject go in ActivateUiElements)
            {
                go.SetActive(true);
            }
        }
        base.OnDestroy();
    }
}
