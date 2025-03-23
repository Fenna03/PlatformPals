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

        // Create new player data
        LocalPlayerData newPlayer = new LocalPlayerData(input);

        // Assign a unique skin using LocalGameManager
        newPlayer.skinId = LocalGameManager.Instance.GetNextAvailableSkin();

        // Add the player to lists
        LocalGameManager.Instance.localPlayerData.Add(newPlayer);
        LocalManagerScript newPlayerManager = input.gameObject.GetComponent<LocalManagerScript>();
        LocalGameManager.Instance.Players.Add(newPlayerManager);

        // Apply the assigned skin
        newPlayerManager.SetPlayerSkin(newPlayerManager.playerSkinList[newPlayer.skinId]);

        // Check for duplicate skins
        LocalGameManager.Instance.CheckForDuplicateSkins();
    }
}
