using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class mainMenuCleanup : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (optionsScript.Instance != null)
        {
            Destroy(optionsScript.Instance.gameObject);
        }
    }
}
