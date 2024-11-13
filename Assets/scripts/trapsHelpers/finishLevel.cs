using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class finishLevel : MonoBehaviour
{
    public List<MovingPlayer> players = new List<MovingPlayer>();
    public List<MovingPlayer> touchers = new List<MovingPlayer>();

    public Button levelButton;
    public Text MessageText;
    private void Awake()
    {
        levelButton.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.levelSelect);
        });
    }

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
        touchers.Add(col.gameObject.GetComponent<MovingPlayer>());
        Debug.Log("all players: " + players.Count);
        Debug.Log("touching it: "+ touchers.Count);
        if(touchers.Count == players.Count)
        {
            MessageText.enabled = true;
            MessageText.text = "You Finished the level!!";
            levelButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        touchers.Remove(collision.gameObject.GetComponent<MovingPlayer>());
    }
}
