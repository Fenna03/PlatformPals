using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class characterSelectUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] public Button ReadyButton;

    [SerializeField] private Text lobbyNameText;
    [SerializeField] private Text lobbyCodeText;

    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            // Leave the lobby
            multiplayerGameLobby.Instance.leaveLobby();

            // 🔻 Unsubscribe from event handlers
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= GameManager.Instance.NetworkManager_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= GameManager.Instance.NetworkManager_Server_onClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback -= GameManager.Instance.NetworkManager_Client_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= GameManager.Instance.NetworkManager_Client_onClientDisconnectCallback;
            }

            // Clear player data and visuals
            if (GameManager.Instance.playerDataList != null && NetworkManager.Singleton.IsServer)
            {
                GameManager.Instance.playerDataList.Clear();
            }

            GameManager.Instance.CleanupPlayerCSP();
            GameManager.Instance.ResetOptionsState();

            // Shutdown NetworkManager
            if (NetworkManager.Singleton != null && (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer))
            {
                NetworkManager.Singleton.Shutdown();
            }

            StartCoroutine(DelayedSceneLoad());
        });

        //when player clicks on ready and they don't have the same skin it sets the player to ready
        ReadyButton.onClick.AddListener(() =>
        {
            if (GameManager.Instance.samePlayer == false)
            {
                characterSelectReady.Instance.setPlayerReady();
            }
        });
    }

    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.isOnline = false;
        Loader.Load(Loader.Scene.MainScreen);
    }

    public void Update()
    {
        //if the players don't have the same skin on the button is white otherwise it's gray
        if (GameManager.Instance.samePlayer == false)
        {
            ReadyButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            ReadyButton.GetComponent<Image>().color = Color.gray;
        }
    }

    private void Start()
    {
        //this gets the lobby with lobby name and code.
        Lobby lobby = multiplayerGameLobby.Instance.GetLobby();

        lobbyNameText.text = "Lobby Name: "+ lobby.Name;
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;
    }
}