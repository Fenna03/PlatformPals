using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : NetworkBehaviour
{
    public GameObject optionsMenu;
    public bool paused;

    public void PlayGame(int gameMode = 1)
    {
        PlayerPrefs.SetInt("mode", gameMode);

        if (gameMode == 0)
        {
            SceneManager.LoadScene("start");
        }
        if(gameMode == 1)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("Menu");
        }
        if(gameMode == 2)
        {
            SceneManager.LoadScene("characterSelect");
        }
        if (gameMode == 3)
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        paused = !paused;
        //Time.timeScale = paused ? 0 : 1;
        Debug.Log("Paused state: " + paused);
    }
}
