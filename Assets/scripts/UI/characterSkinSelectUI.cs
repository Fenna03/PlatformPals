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
            optionsScript.Instance.ChangePlayerSkin(skinId);
            UpdateButtonState();
        });
    }

    private void Start()
    {
        optionsScript.Instance.OnPlayerDataNetworkListChanged += Instance_OnPlayerDataNetworkListChanged;
        UpdateIsSelected();
    }

    private void Instance_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        bool isSelected = optionsScript.Instance.GetPlayerData().skinId == skinId;

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        // Disable button if this skin is already selected
        this.gameObject.GetComponent<Button>().interactable = optionsScript.Instance.GetPlayerData().skinId != skinId;
    }
}
