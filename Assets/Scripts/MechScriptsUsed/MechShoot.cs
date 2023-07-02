﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MechShoot : NetworkBehaviour
{

    [SerializeField] GameObject varBullet;
    [SerializeField] GameObject CommandGuidedRocket;
    [SerializeField] GameObject selectedWeapon;
    public WeaponBase selectedWeaponBase;
    private bool FireGun = false;
    List<GameObject> mechWeapons = new List<GameObject>();
    public float LookRotationSpeed = 1f;
    public Rigidbody rb;
    public Camera gunnerCam;
    public Transform MechParent;
    public Transform CurrentTarget;
    public bool UseMouse = false;
    public float TimeSenseFires = 0;
    float nextFire = 0.0f;
    private Vector2 _panThisFrame;

    private float oldxRotation = 0f;
    private float oldyRotation = 0f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Awake()
    {
        mechWeapons.Add(varBullet);
        mechWeapons.Add(CommandGuidedRocket);
        selectedWeapon = mechWeapons.First();
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
        rb = GetComponent<Rigidbody>();
        gunnerCam = transform.parent.gameObject.GetComponentInChildren<Camera>();
    }

    public void LockTarget(Transform target)
    {
        CurrentTarget = target;
    }

    [ServerRpc(RequireOwnership = false)]
    public void FireMainGunServerRpc()
    {
        GameObject newRocket = Instantiate(selectedWeapon, transform.position + transform.forward * 2, transform.rotation) as GameObject;
        newRocket.GetComponent<NetworkObject>().Spawn();
        newRocket.GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.VelocityChange);
        ILockTarget lockTarget = newRocket.GetComponent<ILockTarget>();
        if (CurrentTarget != null && lockTarget != null)
            lockTarget.SetTarget(CurrentTarget);
    }

    void Update()
    {
        if (!IsClient) return;
        if (FireGun && selectedWeaponBase.RateOfFire < nextFire)
        {
            FireMainGunServerRpc();
            nextFire = 0;
            FireGun = false;
        }
        else
        {
            nextFire += Time.deltaTime;
        }
        xRotation -= _panThisFrame.y * LookRotationSpeed;
        yRotation += _panThisFrame.x * LookRotationSpeed;
        xRotation = Mathf.Clamp(xRotation, transform.parent.rotation.eulerAngles.x - 20, transform.parent.rotation.eulerAngles.x + 20);
        yRotation = Mathf.Clamp(yRotation, transform.parent.rotation.eulerAngles.y - 40, transform.parent.rotation.eulerAngles.y + 40);
        if (oldxRotation != xRotation || oldyRotation != yRotation)
        {
            SetNewTurnVectorServerRpc(xRotation, yRotation);
        }
        oldxRotation = xRotation;
        oldyRotation = yRotation;
    }

    [ServerRpc(RequireOwnership = false)]
    void SetNewTurnVectorServerRpc(float newX, float newY)
    {
        xRotation = newX;
        yRotation = newY;
        //TurnVector = newTurn;
    }


    void FixedUpdate()
    {
        if (!IsServer) return;
        xRotation = Mathf.Clamp(xRotation, transform.parent.rotation.eulerAngles.x - 20, transform.parent.rotation.eulerAngles.x + 20);
        yRotation = Mathf.Clamp(yRotation, transform.parent.rotation.eulerAngles.y - 40, transform.parent.rotation.eulerAngles.y + 40);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        oldxRotation = xRotation;
        oldyRotation = yRotation;
    }

    public void OnFire1()
    {
        FireGun = true;
    }

    public void OnFire2()
    {
        Debug.Log("Locking");
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 50, Color.red, duration: 1200, depthTest: false);
        if (Physics.Raycast(transform.position, fwd, out objectHit, 50) && objectHit.transform.tag == "TestEnemy")
            LockTarget(objectHit.transform);
    }
    public void OnLook(InputValue input)
    {
        _panThisFrame = input.Get<Vector2>() * 0.125f;
    }

    public void OnChangeWeapon()
    {
        if (!IsClient) return;
        ChangeWeaponServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeWeaponServerRpc()
    {

        int weaponIndex = mechWeapons.IndexOf(selectedWeapon);
        if (weaponIndex == mechWeapons.Count - 1)
            weaponIndex = 0;
        else
            weaponIndex += 1;
        selectedWeapon = mechWeapons[weaponIndex];
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
        nextFire = selectedWeaponBase.RateOfFire + 1f;
    }
}