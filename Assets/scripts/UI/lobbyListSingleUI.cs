using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class lobbyListSingleUI : MonoBehaviour
{
    [SerializeField] private Text lobbyNameText;
    private Lobby lobby;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.joinWithId(lobby.Id);
        });
    }
    public void setLobby(Lobby lobby)
    {
        this.lobby = lobby;
        lobbyNameText.text = lobby.Name;
    }
}
