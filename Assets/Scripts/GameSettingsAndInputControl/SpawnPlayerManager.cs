using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerManager : NetworkBehaviour
{
    public GameState gameState;
    [SerializeField] public GameObject MechPrefab;
    [SerializeField] public GameObject EwoPrefab;
    private GameSettings gameSetting;

    public string SceneName { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    
    public override void OnNetworkSpawn()
    {
        //If we are hosting, then handle the server side for detecting when clients have connected
        //and when their lobby scenes are finished loading.
        if (IsServer)
        {
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
            SceneTransitionHandler.sceneTransitionHandler.SetSceneState(SceneTransitionHandler.SceneStates.Ingame);
            SceneName = sceneName;            
            GameObject mechGo = Instantiate(MechPrefab, new Vector3(250, 1.53f, 300), Quaternion.Euler(0, 45, 0));
            var PilotCamera = mechGo.transform.Find("Main Camera").GetComponent<Camera>();
            var pilotInputCfg = PilotCamera.transform.parent.GetComponent<MechPilotInputConfiguration>();
            GameObject EwoGo = Instantiate(EwoPrefab, new Vector3(250, 20, 300), Quaternion.Euler(90, 0, -45));
            var EWOCamera = EwoGo.GetComponent<Camera>();
            var ewoInputCfg = EWOCamera.GetComponent<EWOInputConfiguration>();
            mechGo.GetComponent<EwoGameObjectReference>().EwoRefeence = EwoGo;
            EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayer = mechGo;
            mechGo.GetComponent<NetworkObject>().Spawn();
            EwoGo.GetComponent<NetworkObject>().Spawn();
            EwoGo.GetComponent<MinimapUserInterfaceControl>().mechPlayerId.Value = mechGo.GetComponent<NetworkObject>().NetworkObjectId;
            mechGo.GetComponent<EwoGameObjectReference>().EwoRefeenceId.Value = EwoGo.GetComponent<NetworkObject>().NetworkObjectId;
            SceneTransitionHandler.sceneTransitionHandler.OnEventLoadedScene -= SetupBattleScene;
            //initStart();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientHasSpawnedPlayerObjectServerRpc(int playerType, NetworkObjectReference networkObjectRef, ServerRpcParams serverRpcAttribute = default)
    {
        var clientId = serverRpcAttribute.Receive.SenderClientId;
        Debug.Log($"Client spawned rpc: {serverRpcAttribute.Receive.SenderClientId} {playerType} {networkObjectRef.NetworkObjectId}");
        var clientRpcArgs = GameObjectUtilityFunctions.CreateSrvParaWithClientId(serverRpcAttribute.Receive.SenderClientId);
        var playerVal = gameState.ClientsWithRoles[clientId];

        if (playerType == 0 && clientId == 0)
            gameSetting.SetPilotActive2ClientRpc(networkObjectRef, clientRpcArgs);
        else if (playerType == 1 && clientId == 1)
            gameSetting.SetEwoActive2ClientRpc(networkObjectRef, clientRpcArgs);
    }
}
