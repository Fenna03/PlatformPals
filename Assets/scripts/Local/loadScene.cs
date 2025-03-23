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

        if (gameMode == 1)
        {
            SceneManager.LoadScene(6);
        }
        if (gameMode == 2)
        {
            SceneManager.LoadScene(7);
        }
        if (gameMode == 3)
        {
            SceneManager.LoadScene(8);
        }
        if (gameMode == 4)
        {
            SceneManager.LoadScene(9);
        }
        if (gameMode == 5)
        {
            SceneManager.LoadScene(10);
        }
    }
}
