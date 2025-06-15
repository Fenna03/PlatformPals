using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookItems : MonoBehaviour
{
    public Animator anim;

    public GameObject showItemContainer;
    public GameObject hideItemContainer;
    public GameObject createLobbyContainer;
    public GameObject lobbyUI;
    public GameObject lobbyContainer;

    public ShowItems showItems;
    public HideItems hideItems;

    public bool hasDisappeared;
    public bool right;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (showItemContainer != null)
        {
            hideItemContainer.SetActive(false);
            showItemContainer.SetActive(false);
            lobbyContainer.SetActive(false);
            createLobbyContainer.SetActive(false);
            lobbyUI.SetActive(false);
            anim.SetBool("isOpening", true);
        }
    }

    private void Update()
    {
        if (hasDisappeared == true && right == false)
        {
            hasDisappeared = false;
            flipLeftBook();
        }
        else if(hasDisappeared == true && right == true)
        {
            hasDisappeared = false;
            flipRightBook();
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

        showItemContainer.SetActive(true);
        showItems.ShowButtons();
        createLobbyContainer.SetActive(true);
    }

    public void flipLeftBook()
    {
        anim.SetBool("flipLeft", true);
    }
    #endregion flipleft

    #region flipRight
    public void OnAnimationFinishedRight()
    {
        right = false;
        anim.SetBool("flipRight", false);

        showItemContainer.SetActive(true);
        activateButtons();
    }

    public void flipRightBook()
    {
        anim.SetBool("flipRight", true);
    }
    #endregion flipRight


    public void OnAnimationFinishedOpening()
    {
        if (showItemContainer != null)
        {
            showItemContainer.SetActive(true);
            activateButtons();
        }

        anim.SetBool("isOpening", false); 
    }

    public void activateButtons()
    {
        showItems.ShowButtons();

        if (showItemContainer.activeSelf == true)
        {
            lobbyContainer.SetActive(true);
            lobbyUI.SetActive(true);
        }
    }

    public void ShowCreateLobbyUI()
    {
        hideItemContainer.SetActive(true);
    }

    public void UnactivateButtons()
    {
        createLobbyContainer.SetActive(false);
        lobbyContainer.SetActive(false);
        lobbyUI.SetActive(false);
    }

    public void BackTolobby()
    {
        right = true;
        hideItemContainer.SetActive(true);
    }
}