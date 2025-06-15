using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class createLobbyUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button publicButton;
    [SerializeField] private Button privateButton;
    [SerializeField] private InputField lobbyNameInputField;

    public BookItems bookItems;

    private void Awake()
    {
        publicButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.createLobby(lobbyNameInputField.text, false);
        });
        privateButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.createLobby(lobbyNameInputField.text, true);
        });
        closeButton.onClick.AddListener(() =>
        {
            bookItems.BackTolobby();
        });
    }

    private void Start()
    {
        hide();
    }
    public void show()
    {
        gameObject.SetActive(true);
    }
    private void hide()
    {
        gameObject.SetActive(false);
    }
}
