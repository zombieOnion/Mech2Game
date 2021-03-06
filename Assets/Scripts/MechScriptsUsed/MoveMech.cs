﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MoveMech : MonoBehaviour {
    
    public float Speed = 0;
    public float TurnSpeed = 0;
    public Vector3 TurnVector;
    public Rigidbody rb;
    public MovementEnum ForwardDirection;
    public MovementEnum SidewaysDirection;
    public float LookRotationSpeed = 1f;
    public GameObject PilotPrefab;
    private Vector3 inputVec = default;

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
        ForwardSpeeds = new float[3] { 0, -15f, 15f };
        SidewaysSpeeds = new float[3] { 0, -20f, 20f };
        TurnVector = new Vector3(0f, 0f, 0f);
        //PlayerInput.Instantiate(PilotPrefab, playerIndex: 0, controlScheme: "KeyboardMouse", pairWithDevices: new InputDevice[] { Keyboard.current, Mouse.current });
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        var newSidewaysDirection= DetermineMovementDirection(inputVec.x);
        if (HasMovemnetDirectionChanged(SidewaysDirection, newSidewaysDirection))
        {
            SidewaysDirection = newSidewaysDirection;
            TurnSpeed = SidewaysSpeeds[(int)newSidewaysDirection];
            TurnVector.Set(0, TurnSpeed, 0);
        }

        var newForwardDirection = DetermineMovementDirection(inputVec.y);
        if (HasMovemnetDirectionChanged(ForwardDirection, newForwardDirection))
        {
            ForwardDirection = newForwardDirection;
            Speed = ForwardSpeeds[(int)newForwardDirection];
        }
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
        Quaternion deltaRotation = Quaternion.Euler(TurnVector * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        rb.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);

        //rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(LookRotationSpeed * -Input.GetAxis("Mouse Y"), LookRotationSpeed * -Input.GetAxis("Mouse X"), 0f));
    }
}
