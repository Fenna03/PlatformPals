using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookItems : MonoBehaviour
{
    public Animator anim;

    public GameObject image2;
    public GameObject lobbyUI;
    public GameObject lobbyContainer;

    public ShowItems showItems;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (image2 != null)
        {
            image2.SetActive(false);
            lobbyContainer.SetActive(false);
            lobbyUI.SetActive(false);
            anim.SetBool("isOpening", true);
        }
    }

    #region close
    public void OnAnimationFinishedClosing()
    {
        anim.SetBool("isClosing", false);
    }

    public void CloseBook()
    {
        anim.SetBool("isClosing", true);
    }
    #endregion close

    #region flipLeft
    public void OnAnimationFinishedLeft()
    {
        anim.SetBool("flipLeft", false);
    }

    public void flipLeftBook()
    {
        anim.SetBool("flipLeft", true);
    }
    #endregion flipleft

    #region flipRight
    public void OnAnimationFinishedRight()
    {
        anim.SetBool("flipRight", false);
    }

    public void flipRightBook()
    {
        anim.SetBool("flipRight", true);
    }
    #endregion flipRight


    public void OnAnimationFinishedOpening()
    {
        if (image2 != null)
        {
            image2.SetActive(true);
            activateButtons();
        }

        anim.SetBool("isOpening", false); 
    }

    public void activateButtons()
    {
        showItems.ShowButtons();

        if (image2.activeSelf == true)
        {
            lobbyContainer.SetActive(true);
            lobbyUI.SetActive(true);
        }
    }
}