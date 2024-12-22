using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Loader : MonoBehaviour
{
    public enum Scene
    {
        start,
        Menu,
        characterSelect,
        levelSelect,
        level1,
        level2,
        level3,
        level4,
        level5,
        level6,
        level7,
        level8,
        level9,
        level10,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;


        SceneManager.LoadScene(Scene.Menu.ToString());
    }

    public static void loadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
