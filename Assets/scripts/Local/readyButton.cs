using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class readyButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("LocalLevelSelect");
        optionsScript.Instance.isLocal = true;
        optionsScript.Instance.isOnline = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainScreen");
        optionsScript.Instance.isLocal = false;
        optionsScript.Instance.isOnline = false;
    }
}
