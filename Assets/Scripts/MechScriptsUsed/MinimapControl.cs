﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//kvar att skapa: public variabler för camSize minimum och maximum

public class MinimapControl : MonoBehaviour {
	//deklarera variabler: objektet, dess component, och variabeln camSize:
	public GameObject minimapCameraGO;
	private Camera minimapCamera;
	public float camSize;

	// Use this for initialization
	void Awake () {
		//här hugger vi objektets komponent Camera och sätter den i variabeln minimapCamera
		minimapCamera = minimapCameraGO.GetComponent<Camera>();
		//camSize = minimapCamera.GetComponent<Camera>.orthographicSize;
	}

	public void OnIncreaseMinimapResolution() {

		camSize = camSize + 10;
		minimapCamera.orthographicSize = camSize;
		Debug.Log(camSize);
	}

	public void OnDecreaseMinimapResolution() {
		if(camSize == 10)
			return;
		camSize = camSize - 10;
		minimapCamera.orthographicSize = camSize;
		Debug.Log(camSize);
	}
}