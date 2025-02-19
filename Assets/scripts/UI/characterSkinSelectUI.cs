using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkinSelectUI : MonoBehaviour
{
    [SerializeField] private int skinId;
    [SerializeField] private Image image;

    private void Awake()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (optionsScript.Instance != null)
            {
                optionsScript.Instance.ChangePlayerSkin(skinId);
                UpdateButtonState();
            }
        });
    }

    private void Start()
    {
        if (optionsScript.Instance != null)
        {
            optionsScript.Instance.OnPlayerDataNetworkListChanged += Instance_OnPlayerDataNetworkListChanged;
            UpdateIsSelected();
        }
    }

    private void Instance_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (optionsScript.Instance == null) return;  // Prevent null reference errors

        bool isSelected = optionsScript.Instance.GetPlayerData().skinId == skinId;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (optionsScript.Instance == null) return;  // Prevent null reference errors

        // Disable button if this skin is already selected
        GetComponent<Button>().interactable = optionsScript.Instance.GetPlayerData().skinId != skinId;
    }

    private void OnDisable()
    {
        if (optionsScript.Instance != null)
        {
            optionsScript.Instance.OnPlayerDataNetworkListChanged -= Instance_OnPlayerDataNetworkListChanged;
        }
    }

    private void OnDestroy()
    {
        if (optionsScript.Instance != null)
        {
            optionsScript.Instance.OnPlayerDataNetworkListChanged -= Instance_OnPlayerDataNetworkListChanged;
        }
    }
}