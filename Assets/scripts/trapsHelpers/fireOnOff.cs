using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireOnOff : MonoBehaviour
{
    public Animator anim;
    public BoxCollider2D boxCollider;

    public MovingPlayer movingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (anim.GetBool("isOff") == false)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (movingPlayer.health >= 1)
                {
                    movingPlayer.health -= 1;
                    Debug.Log(movingPlayer.health);
                }
                else if (movingPlayer.health <= 0)
                {
                    movingPlayer.Die();
                }
            }
        }     
    }

    public void On()
    {
        anim.SetBool("isOff", false);
    }
    public void Off()
    {
        anim.SetBool("isOff", true);
    }
}
