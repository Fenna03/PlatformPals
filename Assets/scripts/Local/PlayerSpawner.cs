using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public PlayerInputManager InputManager;
    public List<GameObject> skinPrefabs; // List of prefabs assigned in the Inspector
    private GameObject instance;

    private void Start()
    {
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        // Loop through each player in LocalGameManager
        if (GameManager.Instance.isLocal == true)
        {
            for (int i = 0; i < LocalGameManager.Instance.localPlayerData.Count; i++)
            {
                LocalPlayerData data = LocalGameManager.Instance.localPlayerData[i];

                // Get the correct prefab based on the skinId
                GameObject prefab = skinPrefabs[data.skinId];

                if (prefab == null)
                {
                    Debug.LogError($"Prefab for skin ID {data.skinId} is missing!");
                    continue;
                }

                // Now, set the correct prefab dynamically in the InputManager
                InputManager.playerPrefab = prefab;

                // Instantiate the player prefab at the spawn position
                instance = InputManager.JoinPlayer(data.playerIndex, data.splitscreen, null, data.device).gameObject;
            }
        }
    }
}