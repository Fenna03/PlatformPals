using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMultiplayerUI : MonoBehaviour
{

    public GameObject pauseScreen;
    private void Start()
    {
        optionsScript.Instance.onMultiplayerGamePaused += Instance_onMultiplayerGamePaused;
        optionsScript.Instance.onMultiplayerGameUnpaused += Instance_onMultiplayerGameUnpaused;

        hide();
    }

    private void Instance_onMultiplayerGameUnpaused(object sender, System.EventArgs e)
    {
        hide();
        Debug.Log("ello");
    }

    private void Instance_onMultiplayerGamePaused(object sender, System.EventArgs e)
    {
        show();
        Debug.Log("byee");
    }

    private void show()
    {
        pauseScreen.gameObject.SetActive(true);
        Debug.Log("show screen");
    }

    private void hide()
    {
        pauseScreen.gameObject.SetActive(false);
        Debug.Log("show screen");
    }
}
