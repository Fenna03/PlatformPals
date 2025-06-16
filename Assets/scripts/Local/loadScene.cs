using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour
{
    public void PlayGame(int gameMode)
    {
        PlayerPrefs.SetInt("mode", gameMode);
        if(gameMode == 0)
        {
            SceneManager.LoadScene("Menu1");
        }
        if (gameMode == 1)
        {
            SceneManager.LoadScene("level1");
        }
        if (gameMode == 2)
        {
            SceneManager.LoadScene("level2");
        }
        if (gameMode == 3)
        {
            SceneManager.LoadScene("level3");
        }
        if (gameMode == 4)
        {
            SceneManager.LoadScene("level4");
        }
        if (gameMode == 5)
        {
            SceneManager.LoadScene("level5");
        }
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
