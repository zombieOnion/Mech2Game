using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//kvar att skapa: public variabler för camSize minimum och maximum

public class MinimapControl : MonoBehaviour {
	//deklarera variabler: objektet, dess component, och variabeln camSize:
	public GameObject minimapCamera;
	public Camera camera;
	public float camSize;

	// Use this for initialization
	void Awake () {
		//här hugger vi objektets komponent Camera och sätter den i variabeln camera
		camera = minimapCamera.GetComponent<Camera>();
		//camSize = minimapCamera.GetComponent<Camera>.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnIncreaseMinimapResolution() {
		camSize = camSize + 10;
		camera.orthographicSize = camSize;
		Debug.Log(camSize);
	}

	public void OnDecreaseMinimapResolution() {
		if(camSize == 0)
			return;
		camSize = camSize - 10;
		camera.orthographicSize = camSize;
		Debug.Log(camSize);
	}
}
