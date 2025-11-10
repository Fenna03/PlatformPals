using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Button Resume;
    public Button options;
    public Button mainMenu;
    public Button levels;
    public GameObject optionsObject;

    private void Awake()
    {
        optionsObject.SetActive(false);
        Resume.onClick.AddListener(() =>
        {
            if (optionsScript.Instance != null)
            {
                // Use the same logic as pressing Escape again
                optionsScript.Instance.TogglePauseGame();
            }
        }); 
        options.onClick.AddListener(() =>
        {
            optionsObject.SetActive(true);
        });

        mainMenu.onClick.AddListener(() =>
        {
            multiplayerGameLobby.Instance?.leaveLobby();

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= optionsScript.Instance.NetworkManager_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= optionsScript.Instance.NetworkManager_Server_onClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback -= optionsScript.Instance.NetworkManager_Client_OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback -= optionsScript.Instance.NetworkManager_Client_onClientDisconnectCallback;

                if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
                {
                    NetworkManager.Singleton.Shutdown();
                }
            }

            optionsScript.Instance?.CleanupPlayerCSP();
            optionsScript.Instance?.ResetOptionsState();

            StartCoroutine(DelayedSceneLoad());
        });

        levels.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.levelSelect);
            Time.timeScale = 1f;
        });
    }
    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        Loader.Load(Loader.Scene.MainScreen);
        optionsScript.Instance.TogglePauseGame();
        Time.timeScale = 1f;
    }
}
