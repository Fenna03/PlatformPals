using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonDiff : MonoBehaviour
{
    public Animator anim;
    public bool player1 = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && player1 == true)
        {
            anim.SetBool("isPressed", true);
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            anim.SetBool("isPressed", false);
        }
    }
}
