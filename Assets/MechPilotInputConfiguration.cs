using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MechPilotInputConfiguration : MonoBehaviour
{
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        SetPilotKeyboardMouse();
    }

    private void SetPilotKeyboardMouse() {
        InputUser user = playerInput.user;
        InputUser.PerformPairingWithDevice(Keyboard.current, user);
        InputUser.PerformPairingWithDevice(Mouse.current, user);
        playerInput.SwitchCurrentControlScheme("PilotKeyboardMouse", new InputDevice[] { Keyboard.current, Mouse.current });
        playerInput.SwitchCurrentActionMap("MechPilot");
    }

    private void SetXboxController() {
        InputUser user = playerInput.user;
        InputUser.PerformPairingWithDevice(Gamepad.current, user);
        playerInput.SwitchCurrentControlScheme("XboxController", Gamepad.current);
        playerInput.SwitchCurrentActionMap("MechPilot");
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.f5Key.wasPressedThisFrame)
            SetPilotKeyboardMouse();
        if(Keyboard.current.f6Key.wasPressedThisFrame)
            SetXboxController();
    }
}
