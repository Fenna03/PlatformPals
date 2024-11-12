using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolineOnOff : MonoBehaviour
{
    public Animator anim;
    public BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
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
