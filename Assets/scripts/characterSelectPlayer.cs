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
        disableImage();

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

    public void EnableImage()
    {
        Debug.Log("letsgo");

        sameCharacter.SetActive(true);

        Debug.Log(sameCharacter.activeSelf);

        if (sameCharacter.activeInHierarchy)
        {
            Debug.Log("The sameCharacter is active in the hierarchy.");
        }
        else
        {
            Debug.Log("The sameCharacter is NOT active in the hierarchy.");
        }

        if (this.gameObject.activeInHierarchy)
        {
            Debug.Log(this.gameObject + " is active in the hierarchy.");
        }
        else
        {
            Debug.Log(this.gameObject + " is NOT active in the hierarchy.");
        }

        //sameCharacter.GetComponent<Text>().enabled = true;
        //Debug.Log(sameCharacter.GetComponent<Text>().enabled);

        //still gives the same error that i cannot get rid of asset
        //Destroy(gameObject);
        // sameCharacter.enabled = true;
    }
    public void disableImage()
    {
        Debug.Log("stoppp");

        //sameCharacter.GetComponent<Text>().enabled = false;
        // Debug.Log(sameCharacter.GetComponent<Text>().enabled);


        sameCharacter.SetActive(false);
        //Debug.Log(sameCharacter.activeSelf);
        //sameCharacter.enabled = false;
    }

    private void Start()
    {
        optionsScript.Instance.OnPlayerDataNetworkListChanged += optionsScript_OnPlayerDataNetworkListChanged;
        characterSelectReady.Instance.onReadyChanged += characterSelectReady_OnreadyChanged;

        
        //Debug.Log(NetworkManager.Singleton.IsServer);
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
        if (optionsScript.Instance.isPlayerIndexConnected(playerIndex))
        {
            Show();

            playerData playerData = optionsScript.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            ReadyGameObject.SetActive(characterSelectReady.Instance.isPlayerReady(playerData.clientId));
            playerVisual.SetPlayerSkin(optionsScript.Instance.GetPlayerSkin(playerData.skinId));
        }
        else
        {
            Hide();
        }
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
