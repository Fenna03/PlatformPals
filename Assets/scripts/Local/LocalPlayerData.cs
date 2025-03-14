using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class LocalPlayerData
{
    public int playerIndex;
    public int splitscreen;
    public InputDevice device;
    public int skinId;

    public LocalPlayerData(PlayerInput input)
    {
        playerIndex = input.playerIndex;
        splitscreen = input.splitScreenIndex;
        device = input.devices[0];
    }
}
