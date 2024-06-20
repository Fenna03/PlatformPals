using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button level1;
    //[SerializeField] private Button level2;
    //[SerializeField] private Button level3;
    //[SerializeField] private Button level4;

    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.Menu);
        });
        characterSelectButton.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.characterSelect);
        });
        level1.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level1);
        });
    }
}
