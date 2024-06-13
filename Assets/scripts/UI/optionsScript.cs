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

    public GameObject optionsMenu;
    public bool paused;
    [SerializeField] private Transform playerPrefab;

    private const int MAX_PLAYER_AMOUNT = 4;

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        
        //mainMenu.onClick.AddListener(() =>
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    Loader.Load(Loader.Scene.Menu);
        //});
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
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void startHost()
    {
        NetworkManager.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.ConnectionApprovalCallback = NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
       // Debug.Log(NetworkManager.Singleton.ConnectedClientsIds.Count);
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

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_onClientDisconnectCallback;
        NetworkManager.NetworkConfig.ConnectionApproval = true;
        Debug.Log(NetworkManager.NetworkConfig.ConnectionApproval);
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_onClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
        Debug.Log("Ello");
    }

    public void TogglePause()
    {
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        paused = !paused;
        //Time.timeScale = paused ? 0 : 1;
        Debug.Log("Paused state: " + paused);

        //isLocalGamePaused = !isLocalGamePaused;

        //if (isLocalGamePaused)
        //{
        //   // PauseGameServerRPC();
        //    optionsMenu.gameObject.SetActive(true);
        //    onLocalGamePaused?.Invoke(this, EventArgs.Empty);
        //}
        //else
        //{
        //    //unPauseGameServerRPC();
        //    optionsMenu.gameObject.SetActive(false);
        //    onLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        //}
    }
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

