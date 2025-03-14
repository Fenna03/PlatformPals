using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalHandler : MonoBehaviour
{
    public void PlayerJoined(PlayerInput input)
    {
        if (SpawnManager.Instance == null)
        {
            Debug.LogError("SpawnManager.Instance is null! Is SpawnManager initialized before PlayerJoined is called?");
            return;
        }

        LocalGameManager.Instance.localPlayerData.Add(new LocalPlayerData(input));
        LocalGameManager.Instance.Players.Add(input);
    }
}