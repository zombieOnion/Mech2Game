using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUiTextOnDesttroy : MonoBehaviour
{
    public GameObject[] DisableUiElements;
    public GameObject[] ActivateUiElements;

    private void OnDestroy()
    {
        if (DisableUiElements != null)
        {
            foreach (GameObject go in DisableUiElements)
                go.SetActive(false);
        }
        if (ActivateUiElements != null)
        {
            foreach (GameObject go2 in ActivateUiElements)
                go2.SetActive(true);
        }
    }
}
