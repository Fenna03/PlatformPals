using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : NetworkBehaviour
{
    public GameObject optionsMenu;
    public bool paused;

    public void PlayGame(int gameMode = 1)
    {
        PlayerPrefs.SetInt("mode", gameMode);

        if (gameMode == 0)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("start");
        }
        if (gameMode == 1)
        {
            SceneManager.LoadScene("Menu");
        }
        if (gameMode == 2)
        {
            SceneManager.LoadScene("characterSelect");
        }
        if (gameMode == 3)
        {
            SceneManager.LoadScene("level1");
        }
    }

    // Modified StartGame to include Unity Services initialization
    public async void StartGame()
    {
        // Ensure Unity Gaming Services are initialized
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            try
            {
                await UnityServices.InitializeAsync();
                await SignInAnonymously();
                Debug.Log("Unity Services initialized successfully.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to initialize Unity Services: " + e);
                return;
            }
        }

        // Wait until NetworkManager and optionsScript are ready
        if (NetworkManager.Singleton == null || optionsScript.Instance == null)
        {
            Debug.LogError("NetworkManager or optionsScript is missing!");
            return;
        }

        Allocation allocation = await allocateRelay();
        if (allocation != null)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
            optionsScript.Instance.startHost();

            // Check if the NetworkManager is operating as expected before changing scenes
            if (NetworkManager.Singleton.IsServer)
            {
                Loader.loadNetwork(Loader.Scene.Menu);
            }
            else
            {
                Debug.LogError("NetworkManager is not running as a server or client.");
            }
        }
        else
        {
            Debug.LogError("Relay allocation failed.");
        }
    }


    // Separate method for Relay allocation
    private async Task<Allocation> allocateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(optionsScript.MAX_PLAYER_AMOUNT - 1);
            return allocation;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError("Relay allocation error: " + e);
            return default;
        }
    }

    // Optional: Sign in anonymously if using Authentication
    private async Task SignInAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    // Quits game mode
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Resets game, just starts it again
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Pauses/unpauses game on Escape
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        paused = !paused;
        Debug.Log("Paused state: " + paused);
    }
}
