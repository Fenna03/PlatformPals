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
        Debug.LogError("Ready text assigned");
        ReadyGameObject = GetComponentInChildren<Text>().gameObject;
        playerVisual = gameObject.GetComponent<playerVisual>();
        kickButton = GameObject.Find("kickButton").GetComponent<Button>();
        sameCharacter = GameObject.Find("exlamation");

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
            return;
        }

        if (optionsScript.Instance.isPlayerIndexConnected(playerIndex))
        {
            Show();

            playerData playerData = optionsScript.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            if (ReadyGameObject != null)
            {
                ReadyGameObject.SetActive(characterSelectReady.Instance.isPlayerReady(playerData.clientId)); //error
            }
            if (playerVisual != null)
            {
                playerVisual.SetPlayerSkin(optionsScript.Instance.GetPlayerSkin(playerData.skinId));//error
            }
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        //error
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);//error
    }
}