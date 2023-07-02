using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static PhysicalSpaceLibrary;

public class MinimapUserInterfaceControl : NetworkBehaviour {
	//deklarera variabler: objektet, dess component, och variabeln camSize:
	public readonly Guid MechRadarComputerSignature = Guid.NewGuid();
	public GameObject minimapCameraGO;
	private Camera minimapCamera;
	public float camSize;
	private RadarSweepScript radarSweeper;
	private RadarTargetComputer targetProcessor;
	//private RadarTrackerScript trackerScript;
	private MechShoot mechShoot;
	private GameObjectUtilityFunctions utility;
	[SerializeField] public LayerMask OnlyPlotLayermask;
	[SerializeField] public LayerMask PlotAndTerrainLayermask;
	[SerializeField] public GameObject mechPlayer = null;
	[SerializeField] public NetworkVariable<ulong> mechPlayerId;

	// Use this for initialization
	void Awake () {
		//här hugger vi objektets komponent Camera och sätter den i variabeln minimapCamera
		utility = gameObject.GetComponent<GameObjectUtilityFunctions>();
		minimapCamera = gameObject.GetComponent<Camera>();
		minimapCamera.orthographicSize = camSize;

	}

	void Start()
	{
		//targetProcessor = mechPlayer.transform.root.GetComponentInChildren<RadarTargetComputer>();
		//trackerScript = gameObject.transform.root.GetComponentInChildren<RadarTrackerScript>();
		//mechShoot = mechPlayer.transform.root.GetComponentInChildren<MechShoot>();
		//targetProcessor.MechRadarComputerSignature = MechRadarComputerSignature;
		//camSize = minimapCamera.GetComponent<Camera>.orthographicSize;}
	}

	public void OnMechPlayerIdChange(ulong oldValue, ulong newValue)
    {
		if (!IsServer) return;
		mechPlayer = utility.FindGameObjectByNetworkObjectId(newValue);
		InitScript();
	}

	public override void OnNetworkSpawn()
	{
		if(IsClient && NetworkManager.Singleton.LocalClientId == 1)
        {
			/*gameObject.GetComponent<Camera>().enabled = true;
			Cursor.lockState = CursorLockMode.Confined;
			var ewoInputCfg = gameObject.GetComponent<EWOInputConfiguration>();
			ewoInputCfg.enabled = true;
			ewoInputCfg.PlayerInput.enabled = true;
			ewoInputCfg.PlayerInput.ActivateInput();
			ewoInputCfg.SetEWOKeyboardMouse();*/
		}
		if (!IsServer)
		{
			mechPlayerId.OnValueChanged += OnMechPlayerIdChange;
			base.OnNetworkSpawn();
			return;
		}
		InitScript();
		base.OnNetworkSpawn();
	}

	public override void OnNetworkDespawn()
	{
		if (!IsServer)
		{
			mechPlayerId.OnValueChanged -= OnMechPlayerIdChange;
		}
		base.OnNetworkDespawn();
	}

	private void InitScript()
    {
		targetProcessor = mechPlayer.transform.root.GetComponentInChildren<RadarTargetComputer>();
		mechShoot = mechPlayer.transform.root.GetComponentInChildren<MechShoot>();
		targetProcessor.MechRadarComputerSignature = MechRadarComputerSignature;
		radarSweeper = mechPlayer.transform.root.Find("RadarSweep").GetComponent<RadarSweepScript>();
	}

	private void Update()
    {
		if (!IsServer) return;
		if(mechPlayer == null) return;
		var mechPos = mechPlayer.transform.position;
		if(mechPos.x != transform.position.x || mechPos.y != transform.position.y)
			transform.position = new Vector3(mechPos.x, transform.position.y, mechPos.z);
	}


	private Vector3 GetClickedWorldPoint(Vector3 point) => minimapCamera.ScreenToWorldPoint(point);
	private (Vector3, Collider) FindGroundAndScan(Vector3 worldPoint) {
		worldPoint.y = Terrain.activeTerrain.SampleHeight(transform.position);
		var clickedTarget = CheckForOverlap(worldPoint, targetProcessor.RadarTarget, targetProcessor.TargetLayermask);
		if(clickedTarget.Length < 1)
			return (worldPoint, null);
		else
			return (worldPoint, clickedTarget[0]); 
	}
	public void OnPointClick1() {
		Vector3 userClicked = Mouse.current.position.ReadValue();
		Vector3 clickedWorldPoint = GetClickedWorldPoint(userClicked);
		OnPointClick1ServerRpc(clickedWorldPoint);
	}

    [ServerRpc(RequireOwnership = false)]
	public void OnPointClick1ServerRpc(Vector3 userClicked, ServerRpcParams serverRpcAttribute = default)
	{
		var clientId = serverRpcAttribute.Receive.SenderClientId;
		(Vector3 point, Collider target) = FindGroundAndScan(userClicked);
		if (target == null)
			targetProcessor.CreateNewTarget(point, clientId);
		else
			targetProcessor.DestroyTarget(target.transform);
	}


	public void OnPointClick2() {
		Vector3 userClicked = Mouse.current.position.ReadValue();
		Vector3 clickedWorldPoint = GetClickedWorldPoint(userClicked);
		OnPointClick2ServerRpc(clickedWorldPoint);
	}

    [ServerRpc(RequireOwnership = false)]
	public void OnPointClick2ServerRpc(Vector3 userClicked, ServerRpcParams serverRpcAttribute = default)
	{
		var clientId = serverRpcAttribute.Receive.SenderClientId;
		(Vector3 _, Collider target) = FindGroundAndScan(userClicked);
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

	public void OnRadarSwitchPower() => OnRadarSwitchPowerServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnRadarSwitchPowerServerRpc() => radarSweeper.RadarOn = !radarSweeper.RadarOn;
	public void OnTerrainMapSwitch() => OnTerrainMapSwitchServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnTerrainMapSwitchServerRpc() => minimapCamera.cullingMask = minimapCamera.cullingMask == OnlyPlotLayermask ? PlotAndTerrainLayermask : OnlyPlotLayermask;
	public void OnIncreaseSweepSpeed() => OnIncreaseSweepSpeedServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnIncreaseSweepSpeedServerRpc() => radarSweeper.IncreaseSweepSpeed();
	public void OnDecreaseSweepSpeed() => OnDecreaseSweepSpeedServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnDecreaseSweepSpeedServerRpc() => radarSweeper.DecreaseSweepSpeed();
	public void OnToggleSectorSweep() => OnToggleSectorSweepServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnToggleSectorSweepServerRpc() => radarSweeper.ToggleSectorSweep();
	public void OnIncreaseSectorSweep() => OnIncreaseSectorSweepServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnIncreaseSectorSweepServerRpc() => radarSweeper.IncreaseSectorSweep();
	public void OnDecreaseSectorSweep() => OnDecreaseSectorSweepServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnDecreaseSectorSweepServerRpc() => radarSweeper.DecreaseSectorSweep();
	public void OnRotateSectorSweepForward() => OnRotateSectorSweepForwardServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnRotateSectorSweepForwardServerRpc() => radarSweeper.RotateSectorSweepForward();
	public void OnRotateSectorSweepBackward() => OnRotateSectorSweepBackwardServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnRotateSectorSweepBackwardServerRpc() => radarSweeper.RotateSectorSweepBackward();
	public void OnJammTarget() => OnJammTargetServerRpc();
	[ServerRpc(RequireOwnership = false)]
	public void OnJammTargetServerRpc() => targetProcessor.JammTarget();


}
