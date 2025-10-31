using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : NetworkBehaviour
{
    //public GameObject optionsMenu;
    public bool paused;

    public void PlayGame(int gameMode = 1)
    {
        PlayerPrefs.SetInt("mode", gameMode);

        if (gameMode == 0)
        {
            SceneManager.LoadScene("start");
        }
        if (gameMode == 1)
        {
            SceneManager.LoadScene("Menu1");
        }

    }

    // Quits game mode
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Resets game, just starts it again
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
