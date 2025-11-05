using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Button Resume;
    public Button options;
    public Button mainMenu;
    public Button levels;
    public GameObject optionsObject;

    private void Awake()
    {
        optionsObject.SetActive(false);
        Resume.onClick.AddListener(() =>
        {
            if (optionsScript.Instance != null)
            {
                // Use the same logic as pressing Escape again
                optionsScript.Instance.TogglePauseGame();
            }
        }); 
        options.onClick.AddListener(() =>
        {
            optionsObject.SetActive(true);
        });
        mainMenu.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainScreen);
            Time.timeScale = 1f;
        });
        levels.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.levelSelect);
            Time.timeScale = 1f;
        });
    }
}
