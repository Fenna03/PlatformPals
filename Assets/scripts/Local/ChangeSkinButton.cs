using UnityEngine;
using UnityEngine.UI;

public class ChangeSkinButton : MonoBehaviour
{
    public int skinId; // Skin ID associated with this button

    public void OnClick()
    {
        int playerIndex = 0; // Set this dynamically based on the selected player

        LocalPlayerData playerData = LocalGameManager.Instance.localPlayerData[playerIndex];
        LocalManagerScript playerManager = LocalGameManager.Instance.Players[playerIndex];

        // Update the player's skin ID
        playerData.skinId = skinId;

        // Change the player's skin
        playerManager.SetPlayerSkin(playerManager.playerSkinList[skinId]);

        // Check for duplicate skins
        LocalGameManager.Instance.CheckForDuplicateSkins();
    }

}

