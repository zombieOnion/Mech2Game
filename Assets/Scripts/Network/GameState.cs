using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState 
{
    public readonly Dictionary<ulong, int[]> ClientsWithRoles;

    public GameState(Dictionary<ulong, int[]> clients, int mechCount)
    {
        ClientsWithRoles = clients;
        MechCount = mechCount;
    }

    public int MechCount { get; }
}
