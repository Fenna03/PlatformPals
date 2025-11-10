using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkinSelectUI : MonoBehaviour
{
    [SerializeField] private int skinId;
    [SerializeField] private Image image;
    private Button button;

    private void Awake()
    {
        if (this == null || gameObject == null)
        {
            Debug.LogError("i am null");
            return;
        }

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangePlayerSkin(skinId);
                UpdateButtonState();
            }
        });
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDataNetworkListChanged += Instance_OnPlayerDataNetworkListChanged;
            UpdateIsSelected();
        }

        // ✅ Subscribe to the ready change event
        if (characterSelectReady.Instance != null)
        {
            characterSelectReady.Instance.onReadyChanged += CharacterSelectReady_OnReadyChanged;
        }

        UpdateButtonInteractable();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDataNetworkListChanged -= Instance_OnPlayerDataNetworkListChanged;
        }

        if (characterSelectReady.Instance != null)
        {
            characterSelectReady.Instance.onReadyChanged -= CharacterSelectReady_OnReadyChanged;
        }
    }

    private void Instance_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateIsSelected();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, EventArgs e)
    {
        UpdateButtonInteractable();
    }

    private void UpdateIsSelected()
    {
        if (GameManager.Instance == null) return;
        bool isSelected = GameManager.Instance.GetPlayerData().skinId == skinId;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (GameManager.Instance == null) return;
        button.interactable = GameManager.Instance.GetPlayerData().skinId != skinId;
    }

    // ✅ New helper method
    private void UpdateButtonInteractable()
    {
        if (characterSelectReady.Instance == null || GameManager.Instance == null) return;

        ulong localClientId = Unity.Netcode.NetworkManager.Singleton.LocalClientId;
        bool isReady = characterSelectReady.Instance.isPlayerReady(localClientId);

        // Disable all skin buttons when ready
        button.interactable = !isReady;
    }
}
