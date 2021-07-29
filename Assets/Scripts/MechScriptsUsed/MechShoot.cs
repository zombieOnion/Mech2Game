using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MechShoot : MonoBehaviour {
    
    Object varBullet;
    Object homingRocket;
    Object selectedWeapon;
    WeaponBase selectedWeaponBase;
    List<Object> mechWeapons = new List<Object>();
    public float LookRotationSpeed = 1f;
    public Rigidbody rb;
    public Camera gunnerCam;
    public Transform MechParent;
    public Transform CurrentTarget;
    public bool UseMouse = false;
    float nextFire = 0.0f;
    private Vector2 _panThisFrame;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Awake() {
        varBullet = Resources.Load("Rocket");
        mechWeapons.Add(varBullet);
        homingRocket = Resources.Load("HomingRocket");
        mechWeapons.Add(homingRocket);
        selectedWeapon = mechWeapons.First();
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
        rb = GetComponent<Rigidbody>();
        gunnerCam = transform.parent.gameObject.GetComponentInChildren<Camera>();
    }

    public void LockTarget() {
        Debug.Log("Locking");
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 50, Color.red, duration: 1200, depthTest: false);
        if (Physics.Raycast(transform.position, fwd, out objectHit, 50) && objectHit.transform.tag == "TestEnemy")
            CurrentTarget = objectHit.transform;
    }

    public void FireMainGun() {
        GameObject newRocket = Instantiate(selectedWeapon, transform.position + transform.forward, transform.rotation) as GameObject;
        ILockTarget lockTarget = newRocket.GetComponent<ILockTarget>();
        if (CurrentTarget != null && lockTarget != null)
            lockTarget.SetTarget(CurrentTarget);
    }

    public void OnFire1() {
        if(Time.time > nextFire) {
            FireMainGun();
            nextFire = Time.time + selectedWeaponBase.RateOfFire;
        }
    }

    public void OnFire2() {
        LockTarget();
    }
    public void OnLook(InputValue input) {
        _panThisFrame = input.Get<Vector2>() * 0.125f;
        xRotation -= _panThisFrame.y * LookRotationSpeed;
        yRotation += _panThisFrame.x * LookRotationSpeed;
        xRotation = Mathf.Clamp(xRotation, -40, 40);
        yRotation = Mathf.Clamp(yRotation, -40, 40);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void OnChangeWeapon() {
        int weaponIndex = mechWeapons.IndexOf(selectedWeapon);
        if(weaponIndex == mechWeapons.Count - 1)
            weaponIndex = 0;
        else
            weaponIndex += 1;
        selectedWeapon = mechWeapons[weaponIndex];
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
    }
}
