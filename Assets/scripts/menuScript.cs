using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public void PlayGame(int gameMode = 1)
    {
        PlayerPrefs.SetInt("mode", gameMode);

        if (gameMode == 0)
        {
            SceneManager.LoadScene("Menu");
        }
        if (gameMode == 1)
        {
            SceneManager.LoadScene("level1");
        }
    }

    //quits gameMode
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
        //SceneManager.LoadScene("menu");
    }

    //resets game, just starts it again
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
