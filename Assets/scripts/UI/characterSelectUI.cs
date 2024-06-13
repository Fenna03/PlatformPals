using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class characterSelectUI : MonoBehaviour
{
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button ReadyButton;

    private void Awake()
    {
        MainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.Menu);
        });

        ReadyButton.onClick.AddListener(() =>
        {
            characterSelectReady.Instance.setPlayerReady();
        });
    }
}
