using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState 
{
    public readonly Dictionary<ulong, int> ClientsWithRoles;

    public GameState(Dictionary<ulong, int> clients)
    {
        ClientsWithRoles = clients;
    }
}
