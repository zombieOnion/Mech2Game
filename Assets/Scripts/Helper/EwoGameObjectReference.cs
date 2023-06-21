using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EwoGameObjectReference : NetworkBehaviour
{
    public GameObject EwoRefeence = null;
    public NetworkVariable<ulong> EwoRefeenceId;
    private GameObjectUtilityFunctions utility;

    private void Awake()
    {
		utility = gameObject.GetComponent<GameObjectUtilityFunctions>();
	}
    public override void OnNetworkSpawn()
	{
		if (!IsServer)
		{
			EwoRefeenceId.OnValueChanged += OnMechPlayerIdChange;
		}
		base.OnNetworkSpawn();
	}

	public override void OnNetworkDespawn()
	{
		if (!IsServer)
		{
			EwoRefeenceId.OnValueChanged -= OnMechPlayerIdChange;
		}
		base.OnNetworkDespawn();
	}

	public void OnMechPlayerIdChange(ulong oldValue, ulong newValue)
	{
		EwoRefeence = utility.FindGameObjectByNetworkObjectId(newValue);
	}
}
