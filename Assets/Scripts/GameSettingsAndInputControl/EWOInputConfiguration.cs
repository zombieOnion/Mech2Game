using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class EWOInputConfiguration : MonoBehaviour
{
    public PlayerInput PlayerInput;
    void Start() {
        PlayerInput = GetComponent<PlayerInput>();
        //SetEWOKeyboardMouse();
    }

    public void SetEWOKeyboardMouse() {
        
        InputUser user = PlayerInput.user;
        user.UnpairDevices();
        InputUser.PerformPairingWithDevice(Keyboard.current, user);
        InputUser.PerformPairingWithDevice(Mouse.current, user);
        PlayerInput.SwitchCurrentControlScheme("Keyboard&Mouse", new InputDevice[] { Keyboard.current, Mouse.current });
        PlayerInput.SwitchCurrentActionMap("ElectronicWarfareOfficer"); 
    }
}
