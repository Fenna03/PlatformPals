using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onOffScript : MonoBehaviour
{
    public Animator anim;
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void On()
    {
        anim.SetBool("isOff", false);
    }
    public virtual void Off()
    {
        anim.SetBool("isOff", true);
    }
}
