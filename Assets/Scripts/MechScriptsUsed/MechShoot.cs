using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MechShoot : MonoBehaviour {
    
    Object varBullet;
    Object CommandGuidedRocket;
    Object selectedWeapon;
    public WeaponBase selectedWeaponBase;
    private bool FireGun = false;
    List<Object> mechWeapons = new List<Object>();
    public float LookRotationSpeed = 1f;
    public Rigidbody rb;
    public Camera gunnerCam;
    public Transform MechParent;
    public Transform CurrentTarget;
    public bool UseMouse = false;
    public float TimeSenseFires = 0;
    float nextFire = 0.0f;
    private Vector2 _panThisFrame;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Awake() {
        varBullet = Resources.Load("Rocket");
        mechWeapons.Add(varBullet);
        CommandGuidedRocket = Resources.Load("CommandGuidedRocket");
        mechWeapons.Add(CommandGuidedRocket);
        selectedWeapon = mechWeapons.First();
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
        rb = GetComponent<Rigidbody>();
        gunnerCam = transform.parent.gameObject.GetComponentInChildren<Camera>();
    }

    public void LockTarget(Transform target) {
        CurrentTarget = target;
    }

    public void FireMainGun() {
        GameObject newRocket = Instantiate(selectedWeapon, transform.position + transform.forward*2, transform.rotation) as GameObject;
        newRocket.GetComponent<Rigidbody>().AddForce(rb.transform.forward * 5, ForceMode.VelocityChange);
        ILockTarget lockTarget = newRocket.GetComponent<ILockTarget>();
        if (CurrentTarget != null && lockTarget != null)
            lockTarget.SetTarget(CurrentTarget);
    }
    
    void Update()
    {
        if (FireGun && selectedWeaponBase.RateOfFire < nextFire)
        {
            FireMainGun();
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
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void OnFire1() {
        FireGun = true;
    }

    public void OnFire2() {
        Debug.Log("Locking");
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd * 50, Color.red, duration: 1200, depthTest: false);
        if(Physics.Raycast(transform.position, fwd, out objectHit, 50) && objectHit.transform.tag == "TestEnemy")
            LockTarget(objectHit.transform); 
    }
    public void OnLook(InputValue input) {
        _panThisFrame = input.Get<Vector2>() * 0.125f;
    }

    public void OnChangeWeapon() {
        int weaponIndex = mechWeapons.IndexOf(selectedWeapon);
        if(weaponIndex == mechWeapons.Count - 1)
            weaponIndex = 0;
        else
            weaponIndex += 1;
        selectedWeapon = mechWeapons[weaponIndex];
        selectedWeaponBase = (selectedWeapon as GameObject).GetComponent<WeaponBase>();
        nextFire = selectedWeaponBase.RateOfFire+1f;
    }
}
