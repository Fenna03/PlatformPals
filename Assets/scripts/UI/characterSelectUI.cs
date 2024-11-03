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
            multiplayerGameLobby.Instance.leaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.Menu);
        });

        ReadyButton.onClick.AddListener(() =>
        {
            if (optionsScript.Instance.samePlayer == false)
            {
                characterSelectReady.Instance.setPlayerReady();
            }
        });
    }

    public void Update()
    {
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
        Lobby lobby = multiplayerGameLobby.Instance.GetLobby();

        lobbyNameText.text = "Lobby Name: "+ lobby.Name;
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;
    }
}
