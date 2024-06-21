using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class connectingUI : MonoBehaviour
{
    private void Start()
    {
        optionsScript.Instance.OnTryingToJoinGame += Multiplayer_OnTryingToJoinGame;
        optionsScript.Instance.OnFailedToJoinGame += Multiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void Multiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Multiplayer_OnTryingToJoinGame(object sender, System.EventArgs e)
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

    private void OnDestroy()
    {
        optionsScript.Instance.OnTryingToJoinGame -= Multiplayer_OnTryingToJoinGame;
        optionsScript.Instance.OnFailedToJoinGame -= Multiplayer_OnFailedToJoinGame;
    }
}
