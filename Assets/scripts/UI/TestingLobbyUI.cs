using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.startHost();
            Loader.loadNetwork(Loader.Scene.characterSelect);
        });

        joinGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.startClient();
        });
    }

}
