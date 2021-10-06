using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWOInputCommandRouter : MonoBehaviour
{
	public GameObject MinimapControlGameObject;
    private MinimapUserInterfaceControl minimap;
    void Start() {
		minimap = MinimapControlGameObject.GetComponent<MinimapUserInterfaceControl>();
	}
    public void OnIncreaseMinimapResolution() {
		minimap.OnIncreaseMinimapResolution();
	}

	public void OnDecreaseMinimapResolution() {
		minimap.OnDecreaseMinimapResolution();
	}
}
