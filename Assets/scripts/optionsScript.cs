using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class optionsScript : NetworkBehaviour
{
    //public static optionsScript Instance { get; private set; }

    public GameObject optionsMenu;
    //private bool isLocalGamePaused = false;
    //public EventHandler onLocalGamePaused;
    //public EventHandler onLocalGameUnpaused;
    //public EventHandler onMultiplayerGamePaused;
    //public EventHandler onMultiplayerGameUnpaused;

    public bool paused;

    //private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    //private Dictionary<ulong, bool> playerPausedDictionary;

    //private void Awake()
    //{
    //    Instance = this;
    //    playerPausedDictionary = new Dictionary<ulong, bool>();
    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    
    public void TogglePause()
    {
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
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

