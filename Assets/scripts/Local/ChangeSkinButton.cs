using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChangeSkinButton : MonoBehaviour
{
    public int skinId; // Skin ID associated with this button
    public int playerIndex; // Index of the player that this button is for
    LocalManagerScript Parent;

    public void Start()
    {
        Parent = GetComponentInParent<LocalManagerScript>();
        if (LocalGameManager.Instance.Players.Contains(Parent))
        {
            playerIndex = LocalGameManager.Instance.Players.IndexOf(Parent);
        }
    }

    public void OnClick()
    {
        // Get the correct player data and manager
        LocalPlayerData playerData = LocalGameManager.Instance.localPlayerData[playerIndex];
        LocalManagerScript playerManager = LocalGameManager.Instance.Players[playerIndex];

        // Update the player's selected skin
        playerData.skinId = skinId;

        // Apply the skin to the correct player
        playerManager.SetPlayerSkin(playerManager.playerSkinList[skinId]);

        // Optional: Check for duplicate skins
        LocalGameManager.Instance.CheckForDuplicateSkins();

    }
}