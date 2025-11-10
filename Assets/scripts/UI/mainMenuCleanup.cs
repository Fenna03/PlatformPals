using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class mainMenuCleanup : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        if (multiplayerGameLobby.Instance != null)
        {
            Destroy(multiplayerGameLobby.Instance.gameObject);
        }
    }
}
