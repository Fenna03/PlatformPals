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
    [SerializeField] private TMP_InputField lobbyNameInputField;

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
            hide();
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
        gameObject.SetActive(true);
    }
}
