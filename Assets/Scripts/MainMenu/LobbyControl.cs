using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LobbyControl : NetworkBehaviour
{
    [SerializeField]
    private string m_InGameSceneName = "MechGameMainDevelopScene";

    // Minimum player count required to transition to next level
    [SerializeField]
    private int m_MinimumPlayerCount = 2;
    [SerializeField]
    private int mechCount = 1;

    public TMP_Text LobbyText;
    private bool m_AllPlayersInLobby;

    private Dictionary<ulong, int[]> m_ClientsInLobby;
    private string m_UserLobbyStatusText;
    private GameObjectUtilityFunctions utility;
    private SpawnPlayerManager spawnPlayerManager;

    public override void OnNetworkSpawn()
    {
        utility = GetComponent<GameObjectUtilityFunctions>();
        spawnPlayerManager = FindAnyObjectByType<SpawnPlayerManager>();
        m_ClientsInLobby = new Dictionary<ulong, int[]>();

        //Always add ourselves to the list at first
        m_ClientsInLobby.Add(NetworkManager.LocalClientId, new int[2] { 0, 0 });

        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsServer)
        {
            m_AllPlayersInLobby = false;

            //Server will be notified when a client connects
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
            SceneTransitionHandler.sceneTransitionHandler.OnClientLoadedScene += ClientLoadedScene;
        }

        //Update our lobby
        GenerateUserStatsForLobby();

        SceneTransitionHandler.sceneTransitionHandler.SetSceneState(SceneTransitionHandler.SceneStates.Lobby);
    }

    private void OnGUI()
    {
        if (LobbyText != null) LobbyText.text = m_UserLobbyStatusText;
    }

    /// <summary>
    ///     GenerateUserStatsForLobby
    ///     Psuedo code for setting player state
    ///     Just updating a text field, this could use a lot of "refactoring"  :)
    /// </summary>
    private void GenerateUserStatsForLobby()
    {
        m_UserLobbyStatusText = string.Empty;
        foreach (var clientLobbyStatus in m_ClientsInLobby)
        {
            m_UserLobbyStatusText += "PLAYER_" + clientLobbyStatus.Key + "          ";
            switch (clientLobbyStatus.Value[0])
            {
                case 0:
                    m_UserLobbyStatusText += "(NOT READY)\n";
                    break;
                case 1:
                    m_UserLobbyStatusText += $"(PILOT READY){clientLobbyStatus.Value[1]}\n";
                    break;
                case 2:
                    m_UserLobbyStatusText += $"(EWO READY){clientLobbyStatus.Value[1]}\n";
                    break;
                default:
                    m_UserLobbyStatusText += "(NOT READY)\n";
                    break;
            }
        }
    }

    /// <summary>
    ///     UpdateAndCheckPlayersInLobby
    ///     Checks to see if we have at least 2 or more people to start
    /// </summary>
    private void UpdateAndCheckPlayersInLobby()
    {
        m_AllPlayersInLobby = m_ClientsInLobby.Count >= m_MinimumPlayerCount;

        foreach (var clientLobbyStatus in m_ClientsInLobby)
        {
            SendClientReadyStatusUpdatesClientRpc(clientLobbyStatus.Key, clientLobbyStatus.Value);
            if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(clientLobbyStatus.Key))

                //If some clients are still loading into the lobby scene then this is false
                m_AllPlayersInLobby = false;
        }

        CheckForAllPlayersReady();
    }

    /// <summary>
    ///     ClientLoadedScene
    ///     Invoked when a client has loaded this scene
    /// </summary>
    /// <param name="clientId"></param>
    private void ClientLoadedScene(ulong clientId)
    {
        if (IsServer)
        {
            if (!m_ClientsInLobby.ContainsKey(clientId))
            {
                m_ClientsInLobby.Add(clientId, new int[2] { 0, 0 });
                GenerateUserStatsForLobby();
            }

            UpdateAndCheckPlayersInLobby();
        }
    }

    /// <summary>
    ///     OnClientConnectedCallback
    ///     Since we are entering a lobby and Netcode's NetworkManager is spawning the player,
    ///     the server can be configured to only listen for connected clients at this stage.
    /// </summary>
    /// <param name="clientId">client that connected</param>
    private void OnClientConnectedCallback(ulong clientId)
    {
        if (IsServer)
        {
            if (!m_ClientsInLobby.ContainsKey(clientId)) m_ClientsInLobby.Add(clientId, new int[2] { 0,0});
            GenerateUserStatsForLobby();

            UpdateAndCheckPlayersInLobby();
        }
    }

    /// <summary>
    ///     SendClientReadyStatusUpdatesClientRpc
    ///     Sent from the server to the client when a player's status is updated.
    ///     This also populates the connected clients' (excluding host) player state in the lobby
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="isReady"></param>
    [ClientRpc]
    private void SendClientReadyStatusUpdatesClientRpc(ulong clientId, int[] isReady)
    {
        if (!IsServer)
        {
            if (!m_ClientsInLobby.ContainsKey(clientId))
                m_ClientsInLobby.Add(clientId, isReady);
            else
                m_ClientsInLobby[clientId] = isReady;
            GenerateUserStatsForLobby();
        }
    }

    /// <summary>
    ///     CheckForAllPlayersReady
    ///     Checks to see if all players are ready, and if so launches the game
    /// </summary>
    private void CheckForAllPlayersReady()
    {
        if (m_AllPlayersInLobby)
        {
            var allPlayersAreReady = true;
            HashSet<int[]> unique = new HashSet<int[]>();
            foreach (var clientLobbyStatus in m_ClientsInLobby)
                if (clientLobbyStatus.Value[0] == 0 || clientLobbyStatus.Value[1] == 0 || !unique.Add(clientLobbyStatus.Value))

                    //If some clients are still loading into the lobby scene then this is false
                    allPlayersAreReady = false;

            //Only if all players are ready
            if (allPlayersAreReady)
            {
                if (m_ClientsInLobby.Any(x => x.Value[1] == 2))
                    mechCount = 2;
                //Remove our client connected callback
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

                //Remove our scene loaded callback
                SceneTransitionHandler.sceneTransitionHandler.OnClientLoadedScene -= ClientLoadedScene;

                SceneTransitionHandler.sceneTransitionHandler.gameState = new GameState(m_ClientsInLobby, mechCount);
                //Transition to the ingame scene
                SceneTransitionHandler.sceneTransitionHandler.SwitchScene(m_InGameSceneName);
            }
        }
    }

    /// <summary>
    ///     PlayerIsReady
    ///     Tied to the Ready button in the InvadersLobby scene
    /// </summary>
    public void PilotIsReady(int team)
    {
        m_ClientsInLobby[NetworkManager.Singleton.LocalClientId] = new int[2] { 1, team };
        if (IsServer)
        {
            UpdateAndCheckPlayersInLobby();
        }
        else
        {
            OnClientIsReadyServerRpc(NetworkManager.Singleton.LocalClientId, new int[2] { 1, team });
        }

        GenerateUserStatsForLobby();
    }

    public void EwoIsReady(int team)
    {
        m_ClientsInLobby[NetworkManager.Singleton.LocalClientId] = new int[2] { 2, team };
        if (IsServer)
        {
            UpdateAndCheckPlayersInLobby();
        }
        else
        {
            OnClientIsReadyServerRpc(NetworkManager.Singleton.LocalClientId, new int[2] { 2, team });
        }

        GenerateUserStatsForLobby();
    }

    /// <summary>
    ///     OnClientIsReadyServerRpc
    ///     Sent to the server when the player clicks the ready button
    /// </summary>
    /// <param name="clientid">clientId that is ready</param>
    [ServerRpc(RequireOwnership = false)]
    private void OnClientIsReadyServerRpc(ulong clientid, int[] playerModeTeam)
    {
        if (m_ClientsInLobby.ContainsKey(clientid))
        {
            m_ClientsInLobby[clientid] = playerModeTeam;
            UpdateAndCheckPlayersInLobby();
            GenerateUserStatsForLobby();
        }
    }
}
