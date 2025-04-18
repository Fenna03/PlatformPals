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

            // Reset the player data list
            if (optionsScript.Instance.playerDataNetworkList != null && NetworkManager.Singleton.IsServer)
            {
                optionsScript.Instance.playerDataNetworkList.Clear();
            }

            // Clean up visuals and reset other state
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
        Loader.Load(Loader.Scene.Menu);
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
