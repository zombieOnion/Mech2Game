using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class EWOInputConfiguration : MonoBehaviour
{
    private InputUser user;
    // Start is called before the first frame update
    void Start() {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputUser user = playerInput.user;
        InputUser.PerformPairingWithDevice(Keyboard.current, user);
        InputUser.PerformPairingWithDevice(Mouse.current, user);
        playerInput.SwitchCurrentActionMap("ElectronicWarfareOfficer");
    }

}
