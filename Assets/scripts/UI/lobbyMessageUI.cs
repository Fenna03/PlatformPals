using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class lobbyMessageUI : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        optionsScript.Instance.OnFailedToJoinGame += Instance_onFailedToJoinGame;
        multiplayerGameLobby.Instance.onCreateLobbyStarted += multiplayerGameLobby_OnCreateLobbyStarted;
        multiplayerGameLobby.Instance.onCreateLobbyFailed += multiplayerGameLobby_OnCreateLobbyFailed;
        multiplayerGameLobby.Instance.onJoinStarted += Instance_onJoinStarted;
        multiplayerGameLobby.Instance.onJoinFailed += Instance_onJoinFailed;
        multiplayerGameLobby.Instance.onQuickJoinFailed += Instance_onQuickJoinFailed;

        Hide();
    }

    private void Instance_onQuickJoinFailed(object sender, EventArgs e)
    {
        showMessage("No lobby found to join.");
    }

    private void Instance_onJoinFailed(object sender, EventArgs e)
    {
        showMessage("Failed to create lobby.");
    }

    private void Instance_onJoinStarted(object sender, EventArgs e)
    {
        showMessage("Joining lobby...");
    }

    private void multiplayerGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        showMessage("Failed to create lobby.");
    }

    private void multiplayerGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        showMessage("Creating lobby...");
    }

    private void Instance_onFailedToJoinGame(object sender, EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            showMessage("Failed to connect");
        }
        else
        {
            showMessage(NetworkManager.Singleton.DisconnectReason);
        }

        Show();

        messageText.text = NetworkManager.Singleton.DisconnectReason;
        if (messageText.text == "")
        {
            messageText.text = "Failed to connect";
        }
    }

    private void showMessage(string message)
    {
        Show();
        messageText.text = message;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        optionsScript.Instance.OnFailedToJoinGame -= Instance_onFailedToJoinGame;
        multiplayerGameLobby.Instance.onCreateLobbyStarted -= multiplayerGameLobby_OnCreateLobbyStarted;
        multiplayerGameLobby.Instance.onCreateLobbyFailed -= multiplayerGameLobby_OnCreateLobbyFailed;
        multiplayerGameLobby.Instance.onJoinStarted -= Instance_onJoinStarted;
        multiplayerGameLobby.Instance.onJoinFailed -= Instance_onJoinFailed;
        multiplayerGameLobby.Instance.onQuickJoinFailed -= Instance_onQuickJoinFailed;

    }
}
