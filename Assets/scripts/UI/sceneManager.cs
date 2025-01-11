using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneManager : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    //2 players
    [SerializeField] private Button level1;
    [SerializeField] private Button level2;
    [SerializeField] private Button level3;
    [SerializeField] private Button level4;
    [SerializeField] private Button level5;
    // 3 players
    [SerializeField] private Button level6;
    [SerializeField] private Button level7;
    [SerializeField] private Button level8;
    [SerializeField] private Button level9;
    [SerializeField] private Button level10;
    //4 players
    [SerializeField] private Button level11;
    [SerializeField] private Button level12;
    [SerializeField] private Button level13;
    [SerializeField] private Button level14;
    [SerializeField] private Button level15;

    private void Awake()
    {
        menuButton.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.Menu);
        });

        #region 2Players
        level1.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level1);
        });
        level2.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level2);
        });
        level3.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level3);
        });
        level4.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level4);
        });
        level5.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level5);
        });
        #endregion 2Players
        #region 3Players
        level6.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level6);
        });
        level7.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level7);
        });
        level8.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level8);
        });
        level9.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level9);
        });
        level10.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level10);
        });
        #endregion 3Players
        #region 4Players
        level11.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level11);
        });
        level12.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level12);
        });
        level13.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level13);
        });
        level14.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level14);
        });
        level15.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.level15);
        });
        #endregion 4Players

    }
}
