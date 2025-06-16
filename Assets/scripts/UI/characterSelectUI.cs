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
                NetworkManager.Singleton.OnClientConnectedCallback -= optionsScript.Instance.NetworkManager_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= optionsScript.Instance.NetworkManager_Server_onClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback -= optionsScript.Instance.NetworkManager_Client_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= optionsScript.Instance.NetworkManager_Client_onClientDisconnectCallback;
            }

            // Clear player data and visuals
            if (optionsScript.Instance.playerDataList != null && NetworkManager.Singleton.IsServer)
            {
                optionsScript.Instance.playerDataList.Clear();
            }

            optionsScript.Instance.CleanupPlayerCSP();
            optionsScript.Instance.ResetOptionsState();

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
            if (optionsScript.Instance.samePlayer == false)
            {
                characterSelectReady.Instance.setPlayerReady();
            }
        });
    }

    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        Loader.Load(Loader.Scene.Menu1);
    }

    public void Update()
    {
        //if the players don't have the same skin on the button is white otherwise it's gray
        if (optionsScript.Instance.samePlayer == false)
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