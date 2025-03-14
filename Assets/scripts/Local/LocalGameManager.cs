using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager Instance;

    public List<LocalPlayerData> localPlayerData = new List<LocalPlayerData>();
    public List<PlayerInput> Players;

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
        if (localPlayerData.Count == 0)
        {
            return;
        }
        else
        {
            //Debug.Log(GetComponent<PlayerInput>());
            //PlayerInput playerObject = GetComponent<PlayerInput>();

            // Check if the player object is not already in the list
            //if (!Players.Contains(playerObject))
            //{
            //    Players.Add(playerObject);
            //    Debug.Log("Player added to the list");
            //}


            for (int i = 0; i < Players.Count; i++)
            {
                Debug.Log(Players[i].transform);
                foreach (LocalPlayerData data in localPlayerData)
                {
                    if (data.playerIndex != 0)
                    {
                        Debug.Log("Set kick button active");
                    }
                }
                GameObject exlamation = Players[i].transform.Find("Exclamation").gameObject;
                exlamation.SetActive(false);
            }
        }
    }
}