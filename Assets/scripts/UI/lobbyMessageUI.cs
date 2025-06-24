using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class lobbyMessageUI : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private Button closeButton;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        optionsScript.Instance.OnFailedToJoinGame += Instance_onFailedToJoinGame;
        multiplayerGameLobby.Instance.onCreateLobbyStarted += multiplayerGameLobby_OnCreateLobbyStarted;
        multiplayerGameLobby.Instance.onCreateLobbyFailed += multiplayerGameLobby_OnCreateLobbyFailed;
        multiplayerGameLobby.Instance.onJoinStarted += Instance_onJoinStarted;
        multiplayerGameLobby.Instance.onJoinFailed += Instance_onJoinFailed;
        multiplayerGameLobby.Instance.onQuickJoinFailed += Instance_onQuickJoinFailed;

        Hide();
    }

    private void Instance_onQuickJoinFailed(object sender, EventArgs e)
    {
        showMessage("No lobby found to join.");
    }

    private void Instance_onJoinFailed(object sender, EventArgs e)
    {
        showMessage("Failed to create lobby.");
    }

    private void Instance_onJoinStarted(object sender, EventArgs e)
    {
        showMessage("Joining lobby...");
    }

    private void multiplayerGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        showMessage("Failed to create lobby.");
    }

    private void multiplayerGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        showMessage("Creating lobby...");
    }

    private void Instance_onFailedToJoinGame(object sender, EventArgs e)
    {
        string message = NetworkManager.Singleton.DisconnectReason;
        if (string.IsNullOrEmpty(message))
        {
            message = "Failed to connect";
        }

        showMessage(message);
    }

    private void showMessage(string message)
    {
        messageText.text = message;
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, 0.3f));
    }

    private void Hide()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        optionsScript.Instance.OnFailedToJoinGame -= Instance_onFailedToJoinGame;
        multiplayerGameLobby.Instance.onCreateLobbyStarted -= multiplayerGameLobby_OnCreateLobbyStarted;
        multiplayerGameLobby.Instance.onCreateLobbyFailed -= multiplayerGameLobby_OnCreateLobbyFailed;
        multiplayerGameLobby.Instance.onJoinStarted -= Instance_onJoinStarted;
        multiplayerGameLobby.Instance.onJoinFailed -= Instance_onJoinFailed;
        multiplayerGameLobby.Instance.onQuickJoinFailed -= Instance_onQuickJoinFailed;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        cg.alpha = startAlpha;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        cg.alpha = endAlpha;
    }
}