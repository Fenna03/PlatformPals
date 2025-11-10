using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedOthersManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnOnlineGamePaused += Instance_OnOnlineGamePaused;
        GameManager.Instance.OnOnlineGameUnpaused += Instance_OnOnlineGameUnpaused;

        Hide();
    }
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnOnlineGamePaused += Instance_OnOnlineGamePaused;
            GameManager.Instance.OnOnlineGameUnpaused += Instance_OnOnlineGameUnpaused;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnOnlineGamePaused -= Instance_OnOnlineGamePaused;
            GameManager.Instance.OnOnlineGameUnpaused -= Instance_OnOnlineGameUnpaused;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnOnlineGamePaused -= Instance_OnOnlineGamePaused;
            GameManager.Instance.OnOnlineGameUnpaused -= Instance_OnOnlineGameUnpaused;
        }
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
