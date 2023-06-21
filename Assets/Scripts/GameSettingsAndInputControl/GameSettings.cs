using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSettings : NetworkBehaviour
{
    public enum SceneCamera { MainCamera = 0, NavCamera = 1, Split = 2 };

    public Camera PilotCamera;
    public Camera EWOCamera;
    public SceneCamera ActiveCamera = SceneCamera.MainCamera;
    [SerializeField] public GameObject MechPrefab;
    [SerializeField] public GameObject EwoPrefab;

    public MechPilotInputConfiguration pilotInputCfg;
    public EWOInputConfiguration ewoInputCfg;

    private void Awake()
    {
        //pilotInputCfg = PilotCamera.transform.parent.GetComponent<MechPilotInputConfiguration>();
        //ewoInputCfg = EWOCamera.GetComponent<EWOInputConfiguration>();
        
    }
    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            /*Debug.Log("client gamesetting");
            SecActiveCameras(SceneCamera.MainCamera);
            pilotInputCfg.PlayerInput.ActivateInput();
            pilotInputCfg.SetPilotKeyboardMouse();*/
            //pilotInputCfg.PlayerInput.DeactivateInput();
            //ewoInputCfg.PlayerInput.DeactivateInput();
        }
        if (IsServer)
        {
            GameObject mechGo = Instantiate(MechPrefab, new Vector3(250, 1.53f, 300), Quaternion.Euler(0, 45, 0));
            PilotCamera = mechGo.transform.Find("Main Camera").GetComponent<Camera>();
            pilotInputCfg = PilotCamera.transform.parent.GetComponent<MechPilotInputConfiguration>();
            GameObject EwoGo = Instantiate(EwoPrefab, new Vector3(250, 20, 300), Quaternion.Euler(90, 0, -45));
            EWOCamera = EwoGo.GetComponent<Camera>();
            ewoInputCfg = EWOCamera.GetComponent<EWOInputConfiguration>();
            mechGo.GetComponent<EwoGameObjectReference>().EwoRefeence = EwoGo;
            EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayer = mechGo;
            mechGo.GetComponent<NetworkObject>().Spawn();
            EwoGo.GetComponent<NetworkObject>().Spawn();
            EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayerId.Value = mechGo.GetComponent<NetworkObject>().NetworkObjectId;
            mechGo.GetComponent<EwoGameObjectReference>().EwoRefeenceId.Value = EwoGo.GetComponent<NetworkObject>().NetworkObjectId;
            //initStart();
        }
        base.OnNetworkSpawn();
    }

    private void initStart()
    {
        SecActiveCameras(SceneCamera.MainCamera);
        SetCamerasSizes(FullscreenRect, FullscreenRect);
        Cursor.lockState = CursorLockMode.Locked;
        pilotInputCfg.PlayerInput.ActivateInput();
        ewoInputCfg.PlayerInput.DeactivateInput();
        pilotInputCfg.SetPilotKeyboardMouse();

    }
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            SecActiveCameras(SceneCamera.MainCamera);
            SetCamerasSizes(FullscreenRect, FullscreenRect);
            Cursor.lockState = CursorLockMode.Locked;
            pilotInputCfg.enabled = true;
            pilotInputCfg.PlayerInput.enabled = true;
            pilotInputCfg.PlayerInput.ActivateInput();
            ewoInputCfg.enabled = false;
            ewoInputCfg.PlayerInput.enabled = false;
            ewoInputCfg.PlayerInput.DeactivateInput();
            pilotInputCfg.SetPilotKeyboardMouse();
        }
        if (Keyboard.current.f2Key.wasPressedThisFrame)
        {
            SecActiveCameras(SceneCamera.NavCamera);
            SetCamerasSizes(FullscreenRect, FullscreenRect);
            Cursor.lockState = CursorLockMode.Confined;
            pilotInputCfg.enabled = false;
            pilotInputCfg.PlayerInput.enabled = false;
            pilotInputCfg.PlayerInput.DeactivateInput();
            ewoInputCfg.enabled = true;
            ewoInputCfg.PlayerInput.enabled = true;
            ewoInputCfg.PlayerInput.ActivateInput();
            ewoInputCfg.SetEWOKeyboardMouse();
        }
        if (Keyboard.current.f3Key.wasPressedThisFrame)
        {
            SetCamerasSizes(new Rect(0, 0, 0.5f, 1), new Rect(0.5f, 0, 0.5f, 1));
            SecActiveCameras(SceneCamera.Split);
        }
            

        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            pilotInputCfg.PlayerInput.ActivateInput();
            ewoInputCfg.PlayerInput.DeactivateInput();
            pilotInputCfg.SetPilotKeyboardMouse();
        }
        if (Keyboard.current.f6Key.wasPressedThisFrame)
        {
            pilotInputCfg.PlayerInput.ActivateInput();
            ewoInputCfg.PlayerInput.DeactivateInput();
            pilotInputCfg.SetXboxController();
        }
        if (Keyboard.current.f7Key.wasPressedThisFrame)
        {
            pilotInputCfg.PlayerInput.DeactivateInput();
            ewoInputCfg.PlayerInput.ActivateInput();
            ewoInputCfg.SetEWOKeyboardMouse();
        }
    }
    private Rect FullscreenRect => new Rect(0, 0, 1, 1);

    private void SetCamerasSizes(Rect pilotRect, Rect ewoRect)
    {
        PilotCamera.rect = pilotRect;
        EWOCamera.rect = ewoRect;
    }

    private void SecActiveCameras(SceneCamera newActiveCamera)
    {

        switch (newActiveCamera)
        {
            case SceneCamera.MainCamera:
                PilotCamera.enabled = true;
                EWOCamera.enabled = false;
                break;
            case SceneCamera.NavCamera:
                PilotCamera.enabled = false;
                EWOCamera.enabled = true;
                break;
            case SceneCamera.Split:
                PilotCamera.enabled = true;
                EWOCamera.enabled = true;
                break;
            default:
                break;
        }

    }

}
