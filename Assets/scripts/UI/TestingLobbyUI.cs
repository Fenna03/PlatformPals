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
            optionsScript.Instance.startHost();
            Loader.loadNetwork(Loader.Scene.characterSelect);
        });

        joinGameButton.onClick.AddListener(() =>
        {
            optionsScript.Instance.startClient();
        });
    }

}
