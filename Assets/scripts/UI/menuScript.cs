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
            Loader.Load(Loader.Scene.start);
            //SceneManager.LoadScene("start");
        }
        if (gameMode == 1)
        {
            Loader.Load(Loader.Scene.Menu1);
            //SceneManager.LoadScene("Menu1");
        }
        if (gameMode == 2)
        {
            Loader.Load(Loader.Scene.Menu1);
            //SceneManager.LoadScene("levelSelect");
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
