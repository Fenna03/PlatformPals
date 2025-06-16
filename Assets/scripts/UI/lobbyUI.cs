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
    [Header("Buttons")]
    [SerializeField] private Button LocalLobbyButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button codeJoinButton;

    [SerializeField] private InputField codeInputField;

    [Header("Containers")]
    [SerializeField] private List<Transform> lobbyContainers; // List of lobbyContainer pages
    [SerializeField] private Transform lobbyTemplate;

    [Header("gameObjects")]
    public GameObject arrowLeft;
    public GameObject arrowRight;

    [Header("scripts")]
    public BookItems bookitems;
    [SerializeField] private createLobbyUI createLobbyUI;

    public bool test;

    private void Awake()
    {
        LocalLobbyButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LocalCharSelect");
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            bookitems.ShowCreateLobbyUI();
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
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
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
        // Clear all containers
        foreach (Transform container in lobbyContainers)
        {
            foreach (Transform child in container)
            {
                if (child == lobbyTemplate) continue;
                Destroy(child.gameObject);
            }
        }

        int containerIndex = 0;
        int itemsPerContainer = 6;
        int itemCount = 0;

        foreach (Lobby lobby in lobbyList)
        {
            // If current container is full, move to next
            if (itemCount >= itemsPerContainer)
            {
                containerIndex++;
                itemCount = 0;
            }

            if (containerIndex >= lobbyContainers.Count)
            {
                Debug.LogWarning("Ran out of containers to place lobbies!");
                break; // Optional: stop or handle overflow
            }

            Transform currentContainer = lobbyContainers[containerIndex];

            Transform lobbyTransform = Instantiate(lobbyTemplate, currentContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<lobbyListSingleUI>().setLobby(lobby);

            itemCount++;
        }

        // Example: activate navigation arrows if more than one container needed
        arrowLeft.SetActive(lobbyList.Count > itemsPerContainer);
        arrowRight.SetActive(lobbyList.Count > itemsPerContainer);
    }

    private void OnDestroy()
    {
        multiplayerGameLobby.Instance.onLobbyListChanged -= multiplayerGameLobby_onLobbyListChangedEventArgs;
    }

#if UNITY_EDITOR
    [ContextMenu("Test Populate Lobbies")]
    private void TestPopulateLobbies()
    {
        List<Lobby> mockLobbies = new List<Lobby>();

        for (int i = 0; i < 8; i++) // Generate 8 fake lobbies
        {
            Lobby fakeLobby = new Lobby(
                id: $"lobby_{i}",
                name: $"Test Lobby {i + 1}",
                players: new List<Player>(), // Empty player list
                maxPlayers: 4,
                isPrivate: false,
                data: null,
                lobbyCode: $"CODE{i + 1}"
            );

            mockLobbies.Add(fakeLobby);
        }

        updateLobby(mockLobbies);
    }
#endif

}
