using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IReusablePrefab
{
    NetworkVariable<float> DisappearTimerMax { get; set; }
}
