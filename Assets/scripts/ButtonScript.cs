using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ButtonScript : NetworkBehaviour
{
    public Animator anim;
    public fanOnOff fanScript;

    //public BoxCollider2D BC;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //GetComponent<BoxCollider2D>().size = new Vector2(2.56f, 1.330697f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Player" || col.gameObject.tag == "player2")
        {
            anim.SetBool("isPressed", true);
            fanScript.On();
           // Debug.Log("yay");
           // GetComponent<BoxCollider2D>().size = new Vector2(2.56f, 0.29f);
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "player2")
        {
            anim.SetBool("isPressed", false);
            anim.SetBool("isreleased", true);
            fanScript.Off();
            //Debug.Log("nay");
            //GetComponent<BoxCollider2D>().size = new Vector2(2.56f, 1.330697f);
        }
    }
}
