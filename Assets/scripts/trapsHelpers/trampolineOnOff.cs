using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolineOnOff : onOffScript
{
    public List<MovingPlayer> players = new List<MovingPlayer>();

    public float speed = 10f;

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
    public override void On()
    {
        anim.SetBool("isOff", true);

    }
    public override void Off()
    {
        anim.SetBool("isOff", false);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (anim.GetBool("isOff") == true)
        {
            if (col.gameObject.CompareTag("Player"))
            {

                Debug.Log("test");
                foreach (MovingPlayer player in players)
                {
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
                }
            }
        }
    }
}
