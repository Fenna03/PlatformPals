using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    GameObject respawner;

    public List<MovingPlayer> dying = new List<MovingPlayer>();

    private void Start()
    {
        respawner = GameObject.Find("Respawner");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("test");
            dying.Add(collision.gameObject.GetComponent<MovingPlayer>());
            if(dying.Count > 0)
            {
                foreach (MovingPlayer player in dying)
                {
                    player.transform.position = respawner.transform.position;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        dying.Remove(collision.gameObject.GetComponent<MovingPlayer>());
    }
}
