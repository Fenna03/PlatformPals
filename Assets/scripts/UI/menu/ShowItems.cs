using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItems : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimationFinishedAppear()
    {
        anim.SetBool("Appear", false);
        gameObject.SetActive(false);
    }

    public void AppearBook()
    {
        anim.SetBool("Appear", true);
    }

    public void ShowButtons()
    {
        AppearBook();
    }
}
