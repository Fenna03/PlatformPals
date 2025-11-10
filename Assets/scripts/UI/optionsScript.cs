using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkManager;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using UnityEngine.InputSystem;

public class optionsScript : NetworkBehaviour
{
    public static optionsScript Instance { get; private set; }

    // Lists
    [SerializeField] private List<GameObject> playerSkinList;
    public List<characterSelectPlayer> playerCSP = new List<characterSelectPlayer>();
    public List<LocalPlayerData> localData;
    public List<playerData> playerDataList = new List<playerData>(); // Changed from NetworkList

    // Player amount and state flags
    public const int MAX_PLAYER_AMOUNT = 4;
    public int TotalPlayers;
    public bool samePlayer;

    // Events
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;
    public event EventHandler OnOnlineGamePaused;
    public event EventHandler OnOnlineGameUnpaused;

    private Dictionary<ulong, bool> playerPausedDictionary;
    private bool isLocalGamePaused;

    public bool isOnline = false;
    public bool isLocal = false;

    public NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    public GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerPausedDictionary = new Dictionary<ulong, bool>();

        playerDataList = new List<playerData>(); // Initialize list
        RefreshPlayerList();
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        var components = FindObjectsOfType<characterSelectPlayer>();

        if (pauseMenu == null)
        {
            var pauseManager = FindObjectOfType<PauseManager>();
            if (pauseManager != null)
            {
                pauseMenu = pauseManager.gameObject;
                pauseMenu.SetActive(false);
            }
        }

        foreach (var component in components)
        {
            if (!playerCSP.Contains(component))
            {
                playerCSP.Add(component);
                TotalPlayers++;
            }
        }

        if (scene.name == "Menu1" && playerCSP.Count != 0)
        {
            CleanupPlayerCSP();
        }
    }

    public void ResetOptionsState()
    {
        Debug.Log("Resetting");
        playerDataList.Clear();
        playerCSP.Clear();
        localData.Clear();
        TotalPlayers = 0;
        samePlayer = false;
    }

    public void CleanupPlayerCSP()
    {
        playerCSP.RemoveAll(player => player == null || player.gameObject == null);
        playerCSP.Clear();
        TotalPlayers = playerCSP.Count;
    }

    private void RefreshPlayerList()
    {
        playerCSP.Clear();
        characterSelectPlayer[] foundPlayers = FindObjectsOfType<characterSelectPlayer>();
        foreach (var player in foundPlayers)
        {
            playerCSP.Add(player);
        }
        TotalPlayers = playerCSP.Count;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        localData.Add(playerInput.gameObject.GetComponent<LocalPlayerData>());
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoadCompleted;
        }
        isGamePaused.OnValueChanged += isGamePaused_OnValueChanged;
    }

    private void isGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if(isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnOnlineGamePaused.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnOnlineGameUnpaused.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnSceneLoadCompleted(string sceneName, LoadSceneMode mode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in clientsCompleted)
        {
            if (GetPlayerDataFromClientId(clientId).Equals(default(playerData))) continue;
            
            SpawnPlayer(clientId);
        }
    }


    private void SpawnPlayer(ulong clientId)
    {
        var playerData = GetPlayerDataFromClientId(clientId);
        if (playerData.Equals(default(playerData)))
        {
            return;
        }

        var prefab = GetPlayerSkin(playerData.skinId);
        var playerObj = Instantiate(prefab, new Vector3(2.0f, 10f, 0), Quaternion.identity);
        playerObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }



    public void startHost()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized or not assigned in the scene.");
            return;
        }

        NetworkManager.NetworkConfig.ConnectionApproval = true;

        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_onClientDisconnectCallback;

        NetworkManager.Singleton.StartHost();
        isOnline = true;
        isLocal = false;
    }
    //public void startHost()
    //{
    //    if (NetworkManager.Singleton == null)
    //    {
    //        Debug.LogError("NetworkManager is not initialized or not assigned in the scene.");
    //        return;
    //    }

    //    // ✅ Check if the NetworkManager is already running
    //    if (NetworkManager.Singleton.IsListening)
    //    {
    //        Debug.LogWarning("[Netcode] A network session is already running — stopping it first.");
    //        NetworkManager.Singleton.Shutdown();

    //        // Wait one frame to ensure it fully stops before restarting
    //        StartCoroutine(RestartHostNextFrame());
    //        return;
    //    }

    //    StartHostInternal();
    //}

    //private IEnumerator RestartHostNextFrame()
    //{
    //    yield return null; // wait one frame
    //    StartHostInternal();
    //}

    //private void StartHostInternal()
    //{
    //    NetworkManager.NetworkConfig.ConnectionApproval = true;

    //    NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionApprovalCallback;
    //    NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
    //    NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_onClientDisconnectCallback;

    //    NetworkManager.Singleton.StartHost();
    //    isOnline = true;
    //    isLocal = false;

    //    Debug.Log("[Netcode] Host started successfully.");
    //}


    public void NetworkManager_Server_onClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].clientId == clientId)
            {
                playerDataList.RemoveAt(i);
                OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
                break;
            }
        }
    }

    public void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        playerDataList.Add(new playerData
        {
            clientId = clientId,
            skinId = GetFirstUnusedSkinId(),
        });

        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);

        // Set the player’s Unity Services player ID
        setPlayerIdServerRPC(AuthenticationService.Instance.PlayerId);

        // ✅ After updating, sync the full list to all clients
        if (IsServer)
        {
            SyncPlayerDataClientRpc(playerDataList.ToArray());
        }
    }


    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (request.ClientNetworkId == NetworkManager.Singleton.LocalClientId)
        {
            response.Approved = true;
            return;
        }

        if (SceneManager.GetActiveScene().name != Loader.Scene.characterSelect.ToString())
        {
            response.Approved = false;
            response.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            response.Approved = false;
            response.Reason = "Game is full";
            return;
        }

        response.Approved = true;
    }

    public void startClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_onClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;

        NetworkManager.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.StartClient();
    }

    public void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
    {
        setPlayerIdServerRPC(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerIdServerRPC(string playerId, ServerRpcParams rpcParams = default)
    {
        int index = GetPlayerDataIndexFromClientId(rpcParams.Receive.SenderClientId);
        if (index >= 0)
        {
            var data = playerDataList[index];
            data.playerId = playerId;
            playerDataList[index] = data;
            OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void NetworkManager_Client_onClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public bool isPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }

    public playerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (playerData data in playerDataList)
        {
            if (data.clientId == clientId)
            {
                return data;
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
        return playerDataList[playerIndex];
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
    private void ChangePlayerSkinServerRpc(int skinId, ServerRpcParams rpcParams = default)
    {
        ChangePlayerSkinClientRpc(skinId);
        int index = GetPlayerDataIndexFromClientId(rpcParams.Receive.SenderClientId);
        if (index >= 0)
        {
            var data = playerDataList[index];
            data.skinId = skinId;
            playerDataList[index] = data;
        }

        // Broadcast to everyone
        SyncPlayerDataClientRpc(playerDataList.ToArray());
    }


    [ClientRpc]
    void ChangePlayerSkinClientRpc(int skinId)
    {
        foreach (characterSelectPlayer player in playerCSP)
        {
            if (!IsSkinAvailable(skinId))
            {
                player.EnableImage();
                samePlayer = true;
            }
            else
            {
                player.DisableImage();
                samePlayer = false;
            }
        }
    }

    [ClientRpc]
    private void SyncPlayerDataClientRpc(playerData[] allData)
    {
        playerDataList.Clear();
        playerDataList.AddRange(allData);
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool IsSkinAvailable(int skinId)
    {
        foreach (var data in playerDataList)
        {
            if (data.skinId == skinId)
            {
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

    public void TogglePauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (isLocalGamePaused)
        {
            PauseGameServerRPC();

            if(pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRPC();

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                isGamePaused.Value = true;
                //is paused
                return;
            }
        }

        isGamePaused.Value = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pauseMenu != null)
        {
            Destroy(pauseMenu);
            pauseMenu = null;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}