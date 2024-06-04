using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonDiff : MonoBehaviour
{
    public Animator anim;
    private bool player = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            anim.SetBool("isPressed", true);
        }
        //if (col.gameObject.tag == "player2")
        //{
        //    anim.SetBool("isPressed", true);
        //}
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "player2")
        {
            anim.SetBool("isPressed", false);
        }
    }
}
