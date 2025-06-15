using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideItems : MonoBehaviour
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
        bookItems.hasDisappeared = true;
        bookItems.UnactivateButtons();
        gameObject.SetActive(false);
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
