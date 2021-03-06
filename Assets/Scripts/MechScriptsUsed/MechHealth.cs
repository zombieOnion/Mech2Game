﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechHealth : MonoBehaviour, IHealth
{

    private int _health = 100;

    int IHealth.Health
    {
        get { return _health; }
    }

    public void DoDamage(int damageAmount)
    {
        _health -= damageAmount;
        if(_health < 1)
        {
            Destroy(this.gameObject);
        }
        print("_health is" + _health.ToString());
    }

    void OnDestroy()
    {
        print("Mech was destroyed");
    }
}
