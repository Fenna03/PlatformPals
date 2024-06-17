using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class lobbyUI : MonoBehaviour
{
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button codeJoinButton;
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private createLobbyUI createLobbyUI;

    private void Awake()
    {
        createLobbyButton.onClick.AddListener(() =>
        {
            createLobbyUI.show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.quickJoin();
        });
        codeJoinButton.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance.joinWithCode(codeInputField.text);
        });
    }
}
