using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Unity.Netcode.NetworkManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Authentication;
using UnityEngine.TextCore.Text;
using System.Linq;
using UnityEngine.InputSystem;

public class optionsScript : NetworkBehaviour
{
    public static optionsScript Instance { get; private set; }
    //lists
    [SerializeField] private List<GameObject> playerSkinList;
    public List<characterSelectPlayer> playerCSP = new List<characterSelectPlayer>();
    public List<LocalPlayerData> localData;
    private NetworkList<playerData> playerDataNetworkList;

    //playerAmount things
    public const int MAX_PLAYER_AMOUNT = 4;
    public int TotalPlayers;
    public bool samePlayer;

    //eventHandlers
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;

    private void Awake()
    {
        // Set this instance as the singleton instance
        Instance = this;

        // Ensure this object persists across scene changes
        DontDestroyOnLoad(gameObject);

        // Initialize the player data network list
        playerDataNetworkList = new NetworkList<playerData>();
        // Subscribe to the event triggered when the list changes
        playerDataNetworkList.OnListChanged += playerDataNetworkList_onListChanged;
    }

    private void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        // Find all characterSelectPlayer components in the scene
        var components = FindObjectsOfType<characterSelectPlayer>();

        foreach (var component in components)
        {
            // Add the component to the list if it's not already included
            if (!playerCSP.Contains(component))
            {
                playerCSP.Add(component);
                TotalPlayers++; // Update total players count
            }
        }

        if (scene.name == "Menu" && playerCSP.Count != 0)
        {
            CleanupPlayerCSP();
        }
    }

    public void CleanupPlayerCSP()
    {
        playerCSP.RemoveAll(player => player == null || player.gameObject == null); // Remove invalid references
        playerCSP.Clear(); // Now clear the list properly
        TotalPlayers = playerCSP.Count; // This should now be 0
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        localData.Add(playerInput.gameObject.GetComponent<LocalPlayerData>());
    }


    private void playerDataNetworkList_onListChanged(NetworkListEvent<playerData> changeEvent)
    {
        // Invoke the event to notify listeners when the player data list changes
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void OnNetworkSpawn()
    {
        // Only the server should handle scene load events
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        // Instantiate and spawn a player object for each connected client
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(GetPlayerSkin(GetPlayerDataFromClientId(clientId).skinId).transform, new Vector3(2.0f, 10f, 0), Quaternion.identity);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    public void startHost()
    {
        // Ensure NetworkManager is initialized
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not initialized or not assigned in the scene.");
            return;
        }

        // Enable connection approval for clients
        NetworkManager.NetworkConfig.ConnectionApproval = true;

        // Set connection approval and client event callbacks
        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_onClientDisconnectCallback;

        // Start hosting the game
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_onClientDisconnectCallback(ulong clientId)
    {
        // Remove the disconnected client from the player data list
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                playerDataNetworkList.RemoveAt(i);
                break;
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        // Add the new client to the player data list with an available skin
        playerDataNetworkList.Add(new playerData
        {
            clientId = clientId,
            skinId = GetFirstUnusedSkinId(),
        });

        // Assign the player ID on the server
        setPlayerIdServerRPC(AuthenticationService.Instance.PlayerId);
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        // Prevent rejecting the host connection
        if (connectionApprovalRequest.ClientNetworkId == NetworkManager.Singleton.LocalClientId)
        {
            connectionApprovalResponse.Approved = true;
            return;
        }

        // Reject connection if the game has already started
        if (SceneManager.GetActiveScene().name != Loader.Scene.characterSelect.ToString())
        {
            Debug.LogError("Game has already started, rejecting connection.");
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }

        // Reject connection if the game is full
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }

        // Approve the connection
        connectionApprovalResponse.Approved = true;
    }

    public void startClient()
    {
        // Notify that the client is attempting to join
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        // Set client connection and disconnection event callbacks
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_onClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;

        // Enable connection approval and start the client
        NetworkManager.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
    {
        // Set the player ID on the server when the client connects
        setPlayerIdServerRPC(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerIdServerRPC(string playerId, ServerRpcParams serverRpcParams = default)
    {
        // Find the index of the player in the network list
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        // Update the player data with the new player ID
        playerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.playerId = playerId;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    private void NetworkManager_Client_onClientDisconnectCallback(ulong clientId)
    {
        // Notify that the client failed to join the game
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public bool isPlayerIndexConnected(int playerIndex)
    {
        // Check if the given player index is connected
        return playerIndex < playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        // Find the index of a player based on their client ID
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
        // Find and return player data based on client ID
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
        // Return player data for the local client
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public playerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        // Return player data for a given player index
        return playerDataNetworkList[playerIndex];
    }

    public GameObject GetPlayerSkin(int skinId)
    {
        // Return the GameObject representing a player's skin
        return playerSkinList[skinId];
    }

    public void ChangePlayerSkin(int skinId)
    {
        // Request the server to change the player's skin
        ChangePlayerSkinServerRpc(skinId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerSkinServerRpc(int skinId, ServerRpcParams serverRpcParams = default)
    {
        // Notify all clients to change the skin
        ChangePlayerSkinClientRpc(skinId);

        // Update the player's skin in the network list
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        playerData playerData = playerDataNetworkList[playerDataIndex];
        playerData.skinId = skinId;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ClientRpc]
    void ChangePlayerSkinClientRpc(int skinId)
    {
        //Iterate through players and update their skin availability
        foreach (characterSelectPlayer ready in playerCSP)
        {
            if (!IsSkinAvailable(skinId))
            {
                ready.EnableImage(); // Indicate skin is in use
                samePlayer = true;
            }
            else
            {
                ready.DisableImage(); // Indicate skin is available
                samePlayer = false;
            }
        }
    }

    private bool IsSkinAvailable(int skinId)
    {
        // Check if a skin is already in use
        foreach (playerData playerData in playerDataNetworkList)
        {
            if (playerData.skinId == skinId)
            {
                return false; // Skin is taken
            }
        }
        return true; // Skin is available
    }

    private int GetFirstUnusedSkinId()
    {
        // Find the first available skin ID
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
        // Disconnect a player from the server and remove their data
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_onClientDisconnectCallback(clientId);
    }
}