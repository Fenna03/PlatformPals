using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public List<Transform> spawnPoints; // Assign spawn points in Unity Editor
    private int nextSpawnIndex = 0;
    private bool nextScene = false;

    public int totalPlayers;

    private void Awake()
    {
        // Singleton pattern for global access
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Subscribe to PlayerInputManager's event
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(3);
        nextScene = true;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (nextScene == false)
        {
            // Check if spawn points are set
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points assigned! Please add spawn points to the SpawnManager.");
                return;
            }

            // Get the next spawn point
            Transform spawnPoint = GetNextSpawnPoint();

            // Set the player's position to the spawn point
            if (spawnPoint != null)
            {
                playerInput.gameObject.transform.position = spawnPoint.position;
                playerInput.gameObject.transform.rotation = spawnPoint.rotation;
            }
            totalPlayers++;
        }
    }

    // Method to get the next spawn point
    public Transform GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return null;
        }

        // Get the spawn point and cycle to the next index
        Transform spawnPoint = spawnPoints[nextSpawnIndex];
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Count;
        return spawnPoint;
    }
}
