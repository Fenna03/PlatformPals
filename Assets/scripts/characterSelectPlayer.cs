using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class characterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject ReadyGameObject;
    [SerializeField] private playerVisual playerVisual;
    [SerializeField] private Button kickButton;
    [SerializeField] private GameObject sameCharacter;

    private void Awake()
    {
        DisableImage();
        if (kickButton != null)
        {
            kickButton.onClick.RemoveAllListeners();
            kickButton.onClick.AddListener(() =>
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    playerData playerData = optionsScript.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
                    multiplayerGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
                    optionsScript.Instance.kickPlayer(playerData.clientId);
                }
            });
        }
    }

    public void EnableImage()
    {
        // Debug.Log("letsgo");
        sameCharacter.SetActive(true);
    }

    public void DisableImage()
    {
        // Debug.Log("stoppp");
        sameCharacter.SetActive(false);
    }

    private void Start()
    {
        optionsScript.Instance.OnPlayerDataNetworkListChanged += optionsScript_OnPlayerDataNetworkListChanged;
        characterSelectReady.Instance.onReadyChanged += characterSelectReady_OnreadyChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        UpdatePlayer();
    }

    private void characterSelectReady_OnreadyChanged(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void optionsScript_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        // Check if the object is still valid
        if (this == null || gameObject == null)
        {
            Debug.LogError("i am null: " + playerIndex);
            //Instantiate(optionsScript.Instance.playerSkins[playerIndex]);
            //return;
        }

        if (optionsScript.Instance != null && optionsScript.Instance.isPlayerIndexConnected(playerIndex))
        {
            Show();

            playerData playerData = optionsScript.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            if (ReadyGameObject != null)
            {
                ReadyGameObject.SetActive(characterSelectReady.Instance.isPlayerReady(playerData.clientId));
            }
            if (playerVisual != null)
            {
                playerVisual.SetPlayerSkin(optionsScript.Instance.GetPlayerSkin(playerData.skinId));
            }
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true); //error

    }

    private void Hide()
    {
        gameObject.SetActive(false); //error
    }

    private void OnDestroy()
    {
        if (optionsScript.Instance != null)
            optionsScript.Instance.OnPlayerDataNetworkListChanged -= optionsScript_OnPlayerDataNetworkListChanged;

        if (characterSelectReady.Instance != null)
            characterSelectReady.Instance.onReadyChanged -= characterSelectReady_OnreadyChanged;
    }

}