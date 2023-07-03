using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerManager : NetworkBehaviour
{
    public static SpawnPlayerManager Singleton { get; private set; }
    public GameState gameState;
    [SerializeField] public GameObject MechPrefab;
    [SerializeField] public GameObject EwoPrefab;
    private GameSettings gameSetting;
    private Dictionary<ulong, ulong> clientsObject = new Dictionary<ulong, ulong>();
    public NetworkVariable<bool> AllPlayersHaveSpawned;

    public string SceneName { get; private set; }
    public Dictionary<ulong, ulong> ClientsObject { get => clientsObject; }

    // Start is called before the first frame update
    public void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }


    public override void OnNetworkSpawn()
    {
        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsServer)
        {
            AllPlayersHaveSpawned.Value = false;
            gameSetting = FindAnyObjectByType<GameSettings>();
            gameState = SceneTransitionHandler.sceneTransitionHandler.gameState;
            SceneTransitionHandler.sceneTransitionHandler.OnEventLoadedScene += SetupBattleScene;
        }
        base.OnNetworkSpawn();
    }

    public void SetupBattleScene(string sceneName)
    {
        if (IsServer || IsHost)
        {
            var teamCount = gameState.MechCount;
            var offset = 0;
            for (int i = 0; i < teamCount; i++)
            {
                SceneTransitionHandler.sceneTransitionHandler.SetSceneState(SceneTransitionHandler.SceneStates.Ingame);
                SceneName = sceneName;
                GameObject mechGo = Instantiate(MechPrefab, new Vector3(250-offset, 1.53f, 300-offset), Quaternion.Euler(0, 45, 0));
                var PilotCamera = mechGo.transform.Find("Main Camera").GetComponent<Camera>();
                var pilotInputCfg = PilotCamera.transform.parent.GetComponent<MechPilotInputConfiguration>();
                GameObject EwoGo = Instantiate(EwoPrefab, new Vector3(250-offset, 20, 300-offset), Quaternion.Euler(90, 0, -45));
                var EWOCamera = EwoGo.GetComponent<Camera>();
                var ewoInputCfg = EWOCamera.GetComponent<EWOInputConfiguration>();
                mechGo.GetComponent<EwoGameObjectReference>().EwoRefeence = EwoGo;
                EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayer = mechGo;

                var mechClientID = gameState.ClientsWithRoles.Where(cwr => cwr.Value[0] == 1 && cwr.Value[1] == i + 1).ToList();
                if(mechClientID.Count() > 0)
                    mechGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(mechClientID[0].Key, true);
                else
                    mechGo.GetComponent<NetworkObject>().Spawn();

                var ewoClientID = gameState.ClientsWithRoles.Where(cwr => cwr.Value[0] == 2 && cwr.Value[1] == i + 1).ToList();
                if (ewoClientID.Count() > 0)
                    EwoGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(ewoClientID[0].Key, true);
                else
                    EwoGo.GetComponent<NetworkObject>().Spawn();

                EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayerId.Value = mechGo.GetComponent<NetworkObject>().NetworkObjectId;
                mechGo.GetComponent<EwoGameObjectReference>().EwoRefeenceId.Value = EwoGo.GetComponent<NetworkObject>().NetworkObjectId;
                mechGo.GetComponent<ClientPlayerSpawnConnector>().team.Value = i + 1;
                EwoGo.GetComponent<ClientPlayerSpawnConnector>().team.Value = i + 1;
                offset += 20;
                //initStart();
            }
            SceneTransitionHandler.sceneTransitionHandler.OnEventLoadedScene -= SetupBattleScene;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientHasSpawnedPlayerObjectServerRpc(int playerType, int team, NetworkObjectReference networkObjectRef, ServerRpcParams serverRpcAttribute = default)
    {
        var clientId = serverRpcAttribute.Receive.SenderClientId;
        Debug.Log($"Client spawned rpc: {serverRpcAttribute.Receive.SenderClientId} {playerType} {networkObjectRef.NetworkObjectId}");
        var clientRpcArgs = GameObjectUtilityFunctions.CreateSrvParaWithClientId(serverRpcAttribute.Receive.SenderClientId);
        var playerVal = gameState.ClientsWithRoles[clientId];
        if (playerVal[1] != team || !networkObjectRef.TryGet(out NetworkObject targetObject))
            return;
        if (playerType == 1 && playerType == playerVal[0])
        {
            gameSetting.SetPilotActive2ClientRpc(networkObjectRef, clientRpcArgs);
            //targetObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
            ClientsObject.Add(networkObjectRef.NetworkObjectId, clientId);
        }
        else if (playerType == 2 && playerType == playerVal[0])
        {
            gameSetting.SetEwoActive2ClientRpc(networkObjectRef, clientRpcArgs);
            //targetObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
            ClientsObject.Add(networkObjectRef.NetworkObjectId, clientId);
        }

        if (playerType == 1 && playerType != playerVal[0])
            gameSetting.SetPilotGoClientRpc(networkObjectRef, clientRpcArgs);
        else if (playerType == 2 && playerType != playerVal[0])
            gameSetting.SetEwoGoClientRpc(networkObjectRef, clientRpcArgs);
        if (allHaveSpawned())
            AllPlayersHaveSpawned.Value = true;
    }

    private bool allHaveSpawned()
    {
        return ClientsObject.Count == gameState.ClientsWithRoles.Count;
    }
}
