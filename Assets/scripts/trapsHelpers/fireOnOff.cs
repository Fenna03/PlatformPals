using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class fireOnOff : MonoBehaviour
{
    public Animator anim;
    public BoxCollider2D boxCol;

    public MovingPlayer movingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.size = new Vector3(0.16f, 0.32f);
        boxCol.offset = new Vector3(0f, 0f);
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
            }
        }     
    }

    public void On()
    {
        anim.SetBool("isOff", false);
        boxCol.size = new Vector3(0.16f, 0.32f);
        boxCol.offset = new Vector3(0f, 0f);
    }
    public void Off()
    {
        anim.SetBool("isOff", true);
        boxCol.size = new Vector3(0.16f, 0.16f);
        boxCol.offset = new Vector3(0f, -0.08f);
    }
}
