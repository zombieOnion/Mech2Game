using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//kvar att skapa: public variabler för camSize minimum och maximum

public class MinimapUserInterfaceControl : MonoBehaviour {
	//deklarera variabler: objektet, dess component, och variabeln camSize:
	public GameObject minimapCameraGO;
	private Camera minimapCamera;
	public float camSize;
	private RadarSweepScript radarSweeper;
	private RadarTargetProcessor targetProcessor;
	[SerializeField] public LayerMask OnlyPlotLayermask;
	[SerializeField] public LayerMask PlotAndTerrainLayermask;

	// Use this for initialization
	void Awake () {
		//här hugger vi objektets komponent Camera och sätter den i variabeln minimapCamera
		minimapCamera = minimapCameraGO.GetComponent<Camera>();
		radarSweeper = minimapCameraGO.transform.GetComponentInChildren<RadarSweepScript>();
		minimapCamera.orthographicSize = camSize;
		targetProcessor = gameObject.GetComponent<RadarTargetProcessor>();
		//camSize = minimapCamera.GetComponent<Camera>.orthographicSize;
	}

    public void OnSelect() {
		Debug.Log("click");
		Vector3 worldPoint = minimapCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		Ray ray = minimapCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 50, 1 << 6 | 1 << 8)) {
			Debug.Log(hit.point);
			Debug.Log(hit.transform);
		}
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

    public void OnRadarSwitchPower() => radarSweeper.RadarOn = !radarSweeper.RadarOn;
	public void OnTerrainMapSwitch() => minimapCamera.cullingMask = minimapCamera.cullingMask == OnlyPlotLayermask ? PlotAndTerrainLayermask : OnlyPlotLayermask; 
}
