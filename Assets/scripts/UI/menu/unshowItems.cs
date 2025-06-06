using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unshowItems : MonoBehaviour
{
    public Animator anim;

    public BookItems bookItems;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimationFinishedAppear()
    {
        anim.SetBool("Disappear", false);
        bookItems.CloseBook();
        bookItems.UnactivateButtons();
        Destroy(gameObject);
    }

    public void DisappearButtons()
    {
        anim.SetBool("Disappear", true);
    }

    public void HideButtons()
    {
        DisappearButtons();
    }
}
