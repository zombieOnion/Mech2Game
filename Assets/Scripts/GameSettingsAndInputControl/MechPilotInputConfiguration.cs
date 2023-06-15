using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MechPilotInputConfiguration : MonoBehaviour
{
    public PlayerInput PlayerInput;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        //SetXboxController();
    }

    public void SetPilotKeyboardMouse() {
        InputUser user = PlayerInput.user;
        InputUser.PerformPairingWithDevice(Keyboard.current, user);
        InputUser.PerformPairingWithDevice(Mouse.current, user);
        PlayerInput.SwitchCurrentControlScheme("Keyboard&Mouse", new InputDevice[] { Keyboard.current, Mouse.current });
        PlayerInput.SwitchCurrentActionMap("MechPilot");
    }

    public void SetXboxController() {
        InputUser user = PlayerInput.user;
        InputUser.PerformPairingWithDevice(Gamepad.current, user);
        PlayerInput.SwitchCurrentControlScheme("XboxController", Gamepad.current);
        PlayerInput.SwitchCurrentActionMap("MechPilot");
    }
}
