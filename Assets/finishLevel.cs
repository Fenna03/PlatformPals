using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class finishLevel : MonoBehaviour
{
    public List<MovingPlayer> players = new List<MovingPlayer>();
    public List<GameObject> touchers = new List<GameObject>();

    public Button levelButton;
    public Text MessageText;
    private void Start()
    {
        touchers.Clear();
        MessageText.enabled = false;
        levelButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        var components = FindObjectsOfType<MovingPlayer>();
        foreach (var component in components)
        {
            // Check if the component is already in the list
            if (!players.Contains(component))
            {
                // If not, add it to the list
                players.Add(component);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        touchers.Add(col.gameObject);
        Debug.Log("all players: " + players.Count);
        Debug.Log("touching it: "+ touchers.Count);
        if(touchers.Count == players.Count)
        {
            MessageText.text = "You Finished the level!!";
            MessageText.enabled = true;
            levelButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        }
    }
}
