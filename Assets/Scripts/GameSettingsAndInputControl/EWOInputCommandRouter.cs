using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWOInputCommandRouter : MonoBehaviour
{
	public GameObject MinimapControlGameObject;
    private MinimapControl minimap;
    void Start() {
		minimap = MinimapControlGameObject.GetComponent<MinimapControl>();
	}
    public void OnIncreaseMinimapResolution() {
		minimap.OnIncreaseMinimapResolution();
	}

	public void OnDecreaseMinimapResolution() {
		minimap.OnDecreaseMinimapResolution();
	}
}
