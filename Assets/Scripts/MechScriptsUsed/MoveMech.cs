﻿using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MoveMech : NetworkBehaviour {
    
    public float Speed = 0;
    public Vector3 TurnVector;
    public Rigidbody rb;
    public MovementEnum ForwardDirection;
    public MovementEnum SidewaysDirection;
    public float LookRotationSpeed = 1f;
    public GameObject PilotPrefab;
    private Vector3 inputVec = default;
    public bool hasActedOnUpdateSpeed = true;
    public bool hasActedOnUpdateVector = true;

    public enum MovementEnum
    {
        Neutral = 0,
        Minus = 1,
        Positive = 2
    }

    public float[] ForwardSpeeds;
    public float[] SidewaysSpeeds;

    void Awake()
    {
        ForwardSpeeds = new float[3] { 0, -15f, 25f };
        SidewaysSpeeds = new float[3] { 0, -20f, 20f };
        TurnVector = new Vector3(0f, 0f, 0f);
        //PlayerInput.Instantiate(PilotPrefab, playerIndex: 0, controlScheme: "KeyboardMouse", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient && NetworkManager.Singleton.LocalClientId == 0)
        {/*
            gameObject.GetComponentInChildren<Camera>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            var pilotInputCfg = gameObject.GetComponent<MechPilotInputConfiguration>();
            pilotInputCfg.enabled = true;
            pilotInputCfg.PlayerInput.enabled = true;
            pilotInputCfg.PlayerInput.ActivateInput();
            pilotInputCfg.SetPilotKeyboardMouse();*/
        }
        base.OnNetworkSpawn();
    }

    private void Tick()
    {
        if (!IsClient) return;
        if (!hasActedOnUpdateVector)
        {
            SetNewTurnVectorServerRpc(TurnVector);
            hasActedOnUpdateVector = true;
        }
        if (!hasActedOnUpdateSpeed)
        {
            SetNewSpeedServerRpc(Speed);
            hasActedOnUpdateSpeed = true;
        }
        Debug.Log($"Tick: {NetworkManager.LocalTime.Tick}");
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    void SetNewTurnVectorServerRpc(Vector3 newTurn)
    {
        TurnVector = newTurn;
    }

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    void SetNewSpeedServerRpc(float newSpeed)
    {
        Speed = newSpeed;
    }

    private MovementEnum DetermineMovementDirection(float axisFloat)
    {
        MovementEnum newDirection;// Input.GetAxis(axisName);
        if (axisFloat > 0)
            newDirection = MovementEnum.Positive;
        else if (axisFloat < 0)
            newDirection = MovementEnum.Minus;
        else
            newDirection = MovementEnum.Neutral;
        return newDirection;
    }

    private bool HasMovemnetDirectionChanged(MovementEnum oldM, MovementEnum newM)
    {
        return oldM != newM;
    }

    public void OnMove(InputValue input) {
        var moveVec = input.Get<Vector2>();
        inputVec = new Vector3(moveVec.x, moveVec.y, 0);
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            var newSidewaysDirection = DetermineMovementDirection(inputVec.x);
            if (HasMovemnetDirectionChanged(SidewaysDirection, newSidewaysDirection))
            {
                SidewaysDirection = newSidewaysDirection;
                var turnSpeed = SidewaysSpeeds[(int)newSidewaysDirection];
                TurnVector.Set(0, turnSpeed, 0);
                SetNewTurnVectorServerRpc(TurnVector);
            }

            var newForwardDirection = DetermineMovementDirection(inputVec.y);
            if (HasMovemnetDirectionChanged(ForwardDirection, newForwardDirection))
            {
                ForwardDirection = newForwardDirection;
                Speed = ForwardSpeeds[(int)newForwardDirection];
                SetNewSpeedServerRpc(Speed);
            }
        }
        if (!IsServer) return;
        Quaternion deltaRotation = Quaternion.Euler(TurnVector * Time.deltaTime);
        if(TurnVector.y != 0)
            rb.MoveRotation(rb.rotation * deltaRotation);
        if(Speed !=0)
            rb.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);

        //rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(LookRotationSpeed * -Input.GetAxis("Mouse Y"), LookRotationSpeed * -Input.GetAxis("Mouse X"), 0f));
    }
}
