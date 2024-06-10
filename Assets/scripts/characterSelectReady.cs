using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class characterSelectReady : NetworkBehaviour
{
    private Dictionary<ulong, bool> playerReadyDictionary;


    public static characterSelectReady Instance { get; private set; }
    private void Awake()
    {
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
            Loader.loadNetwork(Loader.Scene.level1);
        }
    }
}
