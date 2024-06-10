using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class optionsScript : NetworkBehaviour
{
    public static optionsScript Instance { get; private set; }

    public GameObject optionsMenu;
    public bool localpaused;
    public EventHandler onMultiplayerGamePaused;
    public EventHandler onMultiplayerGameUnpaused;

    private NetworkVariable<bool> isPaused = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> playerPausedDictionary;

    private void Awake()
    {
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public override void OnNetworkSpawn()
    {
        isPaused.OnValueChanged += isPaused_OnValueChanged;
        base.OnNetworkSpawn();
    }

    private void isPaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isPaused.Value)
        {
            Time.timeScale = 0f;
            onMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            onMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TogglePause()
    {
        localpaused = !localpaused;

        if (localpaused)
        {
            PauseGameServerRPC();
            //Time.timeScale = 0;
            optionsMenu.gameObject.SetActive(true);
            Debug.Log("Paused state: " + localpaused);
        }
        else
        {
            unPauseGameServerRPC();
            //Time.timeScale = 1;
            optionsMenu.gameObject.SetActive(false);
            Debug.Log("Paused state: " + localpaused);
        }
        //optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        //Time.timeScale = paused ? 0 : 1;
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
        testGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void unPauseGameServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        testGamePausedState();
    }

    private void testGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                //this player is paused
                isPaused.Value = true;
                return;
            }
        }
        //all players are unpaused
        isPaused.Value = false;
    }
}
