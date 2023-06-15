using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class WeaponBase : NetworkBehaviour {
    public int Damage = 10;
    public float TravelSpeed = 10f;
    public float SelfDestructFloatTime = 30f;
    protected Rigidbody rb;
    public float RateOfFire = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
        Destroy(gameObject, SelfDestructFloatTime);
        AdditionalStartTimeCode();

    }

    protected virtual void AdditionalStartTimeCode() { }

    void OnCollisionEnter(Collision col)
    {
        if (!NetworkManager.Singleton.IsServer || !NetworkObject.IsSpawned)
            return;
        GameObject hitGo = col.gameObject;
        IHealth hitGoHealth = hitGo.GetComponent<IHealth>();
        if (hitGoHealth != null)
        {
            hitGoHealth.DoDamage(Damage);
        }
        NetworkObject.Despawn(true);
    }

    protected void MoveForward()
    {
        if(IsServer)
            rb.MovePosition(transform.localPosition + transform.forward * TravelSpeed * Time.deltaTime);
    }

}
