using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedOthersManager : MonoBehaviour
{
    private void Start()
    {
        optionsScript.Instance.OnOnlineGamePaused += Instance_OnOnlineGamePaused;
        optionsScript.Instance.OnOnlineGameUnpaused += Instance_OnOnlineGameUnpaused;

        Hide();
    }

    private void Instance_OnOnlineGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Instance_OnOnlineGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
