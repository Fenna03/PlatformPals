using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public class characterSelectReady : NetworkBehaviour
{
    public static characterSelectReady Instance { get; private set; }

    private Dictionary<ulong, bool> playerReadyDictionary;

    public event EventHandler onReadyChanged;

    private void Awake()
    {
        if (this == null || gameObject == null)
        {
            Debug.LogError("i am null READY");
            return;
        }
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void setPlayerReady()
    {
        setPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void setPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        // Toggle the player's ready state
        bool isReady = playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
        bool newReadyState = !isReady;
        playerReadyDictionary[clientId] = newReadyState;

        // Sync with all clients
        setPlayerReadyClientRpc(clientId, newReadyState);

        // Check if everyone is ready
        bool allClientsReady = true;
        foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(id) || !playerReadyDictionary[id])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            multiplayerGameLobby.Instance.deleteLobby();
            Loader.loadNetwork(Loader.Scene.levelSelect);
        }
    }

    [ClientRpc]
    private void setPlayerReadyClientRpc(ulong clientId, bool isReady)
    {
        playerReadyDictionary[clientId] = isReady;
        onReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool isPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
