using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class characterSkinSelectUI : MonoBehaviour
{
    [SerializeField] private int skinId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;


    private void Awake()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            optionsScript.Instance.ChangePlayerSkin(skinId);
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

    //this is unused
    private void UpdateIsSelected()
    {
        if (optionsScript.Instance.GetPlayerData().skinId == skinId)
        {
            //selectedGameObject is the sprite of the player itself, don't want to not show that so maybe something else but now unused
            selectedGameObject.SetActive(true);
        }
    }
}
