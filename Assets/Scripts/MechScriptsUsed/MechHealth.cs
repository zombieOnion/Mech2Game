using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MechHealth : NetworkBehaviour, IHealth
{
    public delegate void MechDestroyed();
    public event MechDestroyed OnMechDestroyed;
    private int _health = 100;

    int IHealth.Health
    {
        get { return _health; }
    }

    public void DoDamage(int damageAmount)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        _health -= damageAmount;
        if(_health < 1)
        {
            NetworkObject.Despawn(true);
        }
        print("_health is" + _health.ToString());
    }

    public override void OnDestroy()
    {
        if (!IsServer) { base.OnDestroy(); return; }
        print("Mech was destroyed");
        OnMechDestroyed?.Invoke();
        /*Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, 1 << 8);
         foreach (Collider collider in hitColliders)
            Destroy(collider.gameObject);
        */
        base.OnDestroy();
    }
}
