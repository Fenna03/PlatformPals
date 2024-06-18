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
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isPressed", true);
            anim.SetBool("isReleased", false);
            fanScript.On();
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isPressed", false);
            anim.SetBool("isReleased", true);
            fanScript.Off();
        }
    }
}
