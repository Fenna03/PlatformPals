using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager Instance;

    public List<LocalPlayerData> localPlayerData = new List<LocalPlayerData>();
    public List<LocalManagerScript> Players;
    public Button ReadyButton;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ReadyButton != null)
        {
            if (Players.Count == 0)
            {
                ReadyButton.gameObject.SetActive(false);
            }
            else if (Players.Count >= 1)
            {
                ReadyButton.gameObject.SetActive(true);
            }
        }
    }

    public void CheckForDuplicateSkins()
    {
        Dictionary<int, int> skinCount = new Dictionary<int, int>();

        // Count how many players have each skin
        foreach (LocalPlayerData data in localPlayerData)
        {
            if (skinCount.ContainsKey(data.skinId))
            {
                skinCount[data.skinId]++;
            }
            else
            {
                skinCount[data.skinId] = 1;
            }
        }

        // Enable/Disable Exclamation based on duplicates
        for (int i = 0; i < localPlayerData.Count; i++)
        {
            LocalManagerScript playerManager = Players[i];

            if (skinCount[localPlayerData[i].skinId] > 1)
            {
                playerManager.EnableImage(); // Show exclamation if duplicate
                if(ReadyButton != null)
                {
                    ReadyButton.GetComponent<Image>().color = Color.gray;
                    ReadyButton.interactable = false;
                }
            }
            else
            {
                playerManager.DisableImage(); // Hide exclamation if unique
                if (ReadyButton != null)
                {
                    ReadyButton.GetComponent<Image>().color = Color.white;
                    ReadyButton.interactable = true;
                }
            }
        }
    }
    public int GetNextAvailableSkin()
    {
        HashSet<int> usedSkins = new HashSet<int>();

        // Collect used skin IDs
        foreach (LocalPlayerData data in localPlayerData)
        {
            usedSkins.Add(data.skinId);
        }

        // Check if Players list is empty to prevent out-of-range errors
        if (Players.Count == 0 || Players[0].playerSkinList.Count == 0) //error
        {
            return 0; // Default to skin 0 (or handle differently)
        }

        // Find the first available skin
        for (int i = 0; i < Players[0].playerSkinList.Count; i++)
        {
            if (!usedSkins.Contains(i)) // If skin ID is not taken
            {
                return i;
            }
        }

        // If all skins are taken, default to 0 (or any fallback)
        return 0;
    }
}