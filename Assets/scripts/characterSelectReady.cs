using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
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
        setPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
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
    private void setPlayerReadyClientRpc(ulong clientId)
    {
        playerReadyDictionary[clientId] = true;

        onReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool isPlayerReady(ulong clientId)
    {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }
}
