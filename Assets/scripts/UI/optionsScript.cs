using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class optionsScript : NetworkBehaviour
{
    public static optionsScript Instance { get; private set; }


    //[SerializeField] private Button mainMenu;

    //public GameObject optionsMenu;
    //public bool paused;
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private List<GameObject> playerSkinList;

    private const int MAX_PLAYER_AMOUNT = 4;

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;

    private NetworkList<playerData> playerDataNetworkList;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        
        playerDataNetworkList = new NetworkList<playerData>();
        playerDataNetworkList.OnListChanged += playerDataNetworkList_onListChanged;
    }

    private void playerDataNetworkList_onListChanged(NetworkListEvent<playerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(GetPlayerSkin(GetPlayerDataFromClientId(clientId).skinId).transform, new Vector3( 2.0f, 10f, 0), Quaternion.identity);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
    
    public void startHost()
    {
        NetworkManager.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_onClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_onClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            playerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new playerData
        {
            clientId = clientId,
            skinId = GetFirstUnusedSkinId(),
        });
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.characterSelect.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        connectionApprovalResponse.Approved = true;
        
    }

    public void startClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_onClientDisconnectCallback;
        NetworkManager.NetworkConfig.ConnectionApproval = true;
        Debug.Log(NetworkManager.NetworkConfig.ConnectionApproval);
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_Client_onClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
        Debug.Log("Ello");
    }

    public bool isPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }

    public playerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (playerData playerData in playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData;
            }
        }
        return default;
    }

    public playerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public playerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }

    public GameObject GetPlayerSkin(int skinId)
    {
        return playerSkinList[skinId];
    }

    public void ChangePlayerSkin(int skinId)
    {
        ChangePlayerSkinServerRpc(skinId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerSkinServerRpc(int skinId, ServerRpcParams serverRpcParams = default)
    {
        if (!IsSkinAvailable(skinId))
        {
            // Color not available
            return;
        }

        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        playerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.skinId = skinId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private bool IsSkinAvailable(int skinId)
    {
        foreach (playerData playerData in playerDataNetworkList)
        {
            if (playerData.skinId == skinId)
            {
                // Already in use
                return false;
            }
        }
        return true;
    }

    private int GetFirstUnusedSkinId()
    {
        for (int i = 0; i < playerSkinList.Count; i++)
        {
            if (IsSkinAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }

    public void kickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_onClientDisconnectCallback(clientId);
    }

    //public void TogglePause()
    //{
    //    optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
    //    paused = !paused;
    //    //Time.timeScale = paused ? 0 : 1;
    //    Debug.Log("Paused state: " + paused);

    //    //isLocalGamePaused = !isLocalGamePaused;

    //    //if (isLocalGamePaused)
    //    //{
    //    //   // PauseGameServerRPC();
    //    //    optionsMenu.gameObject.SetActive(true);
    //    //    onLocalGamePaused?.Invoke(this, EventArgs.Empty);
    //    //}
    //    //else
    //    //{
    //    //    //unPauseGameServerRPC();
    //    //    optionsMenu.gameObject.SetActive(false);
    //    //    onLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
    //    //}
    //}

    //private bool isLocalGamePaused = false;
    //public EventHandler onLocalGamePaused;
    //public EventHandler onLocalGameUnpaused;
    //public EventHandler onMultiplayerGamePaused;
    //public EventHandler onMultiplayerGameUnpaused;

    //private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    //private Dictionary<ulong, bool> playerPausedDictionary;

    //public override void OnNetworkSpawn()
    //{
    //    isGamePaused.OnValueChanged += isPaused_OnValueChanged;
    //    base.OnNetworkSpawn();
    //}

    //private void isPaused_OnValueChanged(bool previousValue, bool newValue)
    //{
    //    if (isGamePaused.Value)
    //    {
    //        Time.timeScale = 0f;
    //        onMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
    //    }
    //    else
    //    {
    //        Time.timeScale = 1f;
    //        onMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
    //    }
    //}


    //[ServerRpc(RequireOwnership = false)]
    //private void PauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    //{
    //    playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
    //    testGamePausedState();
    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void unPauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    //{
    //    playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
    //    testGamePausedState();
    //}

    //private void testGamePausedState()
    //{
    //    foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
    //    {
    //        if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
    //        {
    //            //this player is paused
    //            isGamePaused.Value = true;
    //            return;
    //        }
    //    }
    //    //all players are unpaused
    //    isGamePaused.Value = false;
    //}
}

