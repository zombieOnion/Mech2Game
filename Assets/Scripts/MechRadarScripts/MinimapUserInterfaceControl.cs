using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PhysicalSpaceLibrary;

public class MinimapUserInterfaceControl : MonoBehaviour {
	//deklarera variabler: objektet, dess component, och variabeln camSize:
	public GameObject minimapCameraGO;
	private Camera minimapCamera;
	public float camSize;
	private RadarSweepScript radarSweeper;
	private RadarTargetComputer targetProcessor;
	private MechShoot mechShoot;
	[SerializeField] public LayerMask OnlyPlotLayermask;
	[SerializeField] public LayerMask PlotAndTerrainLayermask;

	// Use this for initialization
	void Awake () {
		//här hugger vi objektets komponent Camera och sätter den i variabeln minimapCamera
		minimapCamera = minimapCameraGO.GetComponent<Camera>();
		radarSweeper = minimapCameraGO.transform.root.GetComponentInChildren<RadarSweepScript>();
		minimapCamera.orthographicSize = camSize;
		targetProcessor = gameObject.transform.root.GetComponentInChildren<RadarTargetComputer>();
		mechShoot = gameObject.transform.root.GetComponentInChildren<MechShoot>();
		//camSize = minimapCamera.GetComponent<Camera>.orthographicSize;
	}

	private Vector3 GetClickedWorldPoint() => minimapCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	private (Vector3, Collider) FindGroundAndScan() {
		Vector3 worldPoint = GetClickedWorldPoint();
		worldPoint.y = Terrain.activeTerrain.SampleHeight(transform.position);
		var clickedTarget = CheckForOverlap(worldPoint, targetProcessor.RadarTarget, targetProcessor.TargetLayermask);
		if(clickedTarget.Length < 1)
			return (worldPoint, null);
		else
			return (worldPoint, clickedTarget[0]); 
	}
	public void OnPointClick1() {
		Debug.Log("click1");
		(Vector3 point, Collider target) = FindGroundAndScan();
		if(target == null)
			targetProcessor.CreateNewTarget(point);
		else
			targetProcessor.DestroyTarget(target.transform);
	}

	public void OnPointClick2() {
		Debug.Log("click2");
		(Vector3 _, Collider target) = FindGroundAndScan();
		if (target == null)
			return;
		targetProcessor.TrackTarget(target.GetComponent<RadarTargetScript>());
		mechShoot.LockTarget(target.transform);
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
	public void OnIncreaseSweepSpeed() => radarSweeper.IncreaseSweepSpeed();
	public void OnDecreaseSweepSpeed() => radarSweeper.DecreaseSweepSpeed();


}
