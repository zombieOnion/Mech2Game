using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState 
{
    public readonly Dictionary<ulong, int[]> ClientsWithRoles;

    public GameState(Dictionary<ulong, int[]> clients, int teamSize)
    {
        ClientsWithRoles = clients;
        TeamSize = teamSize;
    }

    public int TeamSize { get; }
}
