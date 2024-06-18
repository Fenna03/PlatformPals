using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class buttonCheck : NetworkBehaviour
{
    public Animator anim;

    public GameObject button1;
    public GameObject button2;
    public Text messageText;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        messageText.text = "";
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isPressed", true);
            anim.SetBool("isReleased", false);
        }
        if (button1.gameObject.CompareTag("Player") && button2.gameObject.CompareTag("Player"))
        {
            messageText.text = "You finished the test level! Good job!";
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isPressed", false);
            anim.SetBool("isReleased", true);
        }
    }
}
