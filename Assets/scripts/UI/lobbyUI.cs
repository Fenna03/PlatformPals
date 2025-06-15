using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class lobbyUI : MonoBehaviour
{
    [SerializeField] private Button LocalLobbyButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button codeJoinButton;

    [SerializeField] private InputField codeInputField;
    [SerializeField] private createLobbyUI createLobbyUI;

    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;

    public BookItems bookitems;

    private void Awake()
    {
        LocalLobbyButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LocalCharSelect");
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            bookitems.ShowCreateLobbyUI();
            //createLobbyUI.show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.quickJoin();
        });
        codeJoinButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.joinWithCode(codeInputField.text);
        });

        lobbyTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        multiplayerGameLobby.Instance.onLobbyListChanged += multiplayerGameLobby_onLobbyListChangedEventArgs;
        updateLobby(new List<Lobby>());
    }

    private void multiplayerGameLobby_onLobbyListChangedEventArgs(object sender, multiplayerGameLobby.onLobbyListChangedEventArgs e)
    {
        updateLobby(e.lobbyList);
    }

    private void updateLobby(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<lobbyListSingleUI>().setLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        multiplayerGameLobby.Instance.onLobbyListChanged -= multiplayerGameLobby_onLobbyListChangedEventArgs;
    }
}
