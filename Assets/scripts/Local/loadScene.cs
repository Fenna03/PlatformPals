using Unity.Netcode;
using UnityEngine;

public class loadScene : MonoBehaviour
{
    public void PlayGame(int gameMode)
    {
        PlayerPrefs.SetInt("mode", gameMode);

        string sceneName = gameMode switch
        {
            0 => "Menu1",
            1 => "level1",
            2 => "level2",
            3 => "level3",
            4 => "level4",
            5 => "level5",
            6 => "MainScreen",
            _ => "Menu1"
        };

        if (GameManager.Instance.isOnline == true)
        {
            // Use NGO scene manager for synced networked scenes
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            // Local / single-player fallback
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}

