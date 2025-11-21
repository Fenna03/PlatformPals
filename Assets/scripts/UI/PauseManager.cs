using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            if (GameManager.Instance != null)
            {
                // Use the same logic as pressing Escape again
                GameManager.Instance.TogglePauseGame();
            }
        }); 
        options.onClick.AddListener(() =>
        {
            optionsObject.SetActive(true);
        });

        mainMenu.onClick.AddListener(() =>
        {
            if (GameManager.Instance.isOnline)
            {
                multiplayerGameLobby.Instance?.leaveLobby();

                if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
                {
                    NetworkManager.Singleton.Shutdown();
                }

                GameManager.Instance?.CleanupPlayerCSP();
                GameManager.Instance?.ResetOptionsState();

                StartCoroutine(DelayedSceneLoad());
            }
            else if (GameManager.Instance.isLocal)
            {
                SceneManager.LoadScene("MainScreen");
                LocalGameManager.Instance.TogglePause();
                GameManager.Instance.isLocal = false;
                LocalGameManager.Instance.Reset();
            }
        });

        levels.onClick.AddListener(() =>
        {
            if (GameManager.Instance.isOnline)
            {
                GameManager.Instance.TogglePauseGame();
                Time.timeScale = 1f;

                NetworkManager.Singleton.SceneManager.LoadScene(
                    "levelSelect",
                    LoadSceneMode.Additive
                );
            }
            else if (GameManager.Instance.isLocal)
            {
                SceneManager.LoadScene("levelSelect", LoadSceneMode.Additive);
                LocalGameManager.Instance.TogglePause();
                Time.timeScale = 1f;
            }
        });
    }
    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        Loader.Load(Loader.Scene.MainScreen);
        GameManager.Instance.isOnline = false;
        GameManager.Instance.TogglePauseGame();
        Time.timeScale = 1f;
    }

    private IEnumerator LoadLevelsClean()
    {
        yield return new WaitForEndOfFrame();
        Loader.Load(Loader.Scene.levelSelect);
        GameManager.Instance.isOnline = false;
        GameManager.Instance.TogglePauseGame();
        Time.timeScale = 1f;
    }
}
