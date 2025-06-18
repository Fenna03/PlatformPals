using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookItems : MonoBehaviour
{
    public Animator anim;

    [Header("gameObjects")]
    public GameObject showItemContainer;
    public GameObject hideItemContainer;
    public GameObject createLobbyContainer;
    public GameObject lobbyUI;
    public GameObject lobbyContainer;
    public GameObject lobbyContainer2;
    public GameObject lobbyContainer3;
    public GameObject quitContainer;

    [Header("scripts")]
    public ShowItems showItems;
    public HideItems hideItems;
    public lobbyUI lobbyUi;

    [Header("booleans")]
    public bool hasDisappeared;
    public bool right;
    public bool left;
    public bool whichOne;
    public bool backToLobby;

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
            quitContainer.SetActive(false);
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

        if (hasDisappeared == true && left == false)
        {
            hasDisappeared = false;
            flipRightBook();
        }
        else if (hasDisappeared == true && left == true)
        {
            hasDisappeared = false;
            flipLeftBook();
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
        lobbyUi.arrowLeft.SetActive(true);
        lobbyUi.arrowRight.SetActive(true);

        
        if (whichOne == false && backToLobby == false)
        {
            createLobbyContainer.SetActive(true);
        }
        else if(whichOne == true && backToLobby == false)
        {
            lobbyContainer2.SetActive(true);
            lobbyContainer3.SetActive(true);
            whichOne = false;
        }
        else if (backToLobby == true)
        {
            activateButtons();
            backToLobby = false;
        }
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
        lobbyUi.arrowLeft.SetActive(true);
        lobbyUi.arrowRight.SetActive(true);
        activateButtons();
    }

    public void flipRightBook()
    {
        anim.SetBool("flipRight", true);
    }
    #endregion flipRight


    public void OnAnimationFinishedOpening()
    {
        showItemContainer.SetActive(true);
        activateButtons();

        anim.SetBool("isOpening", false);
    }

    public void activateButtons()
    {
        showItems.ShowButtons();

        if (whichOne == false)
        {
            lobbyContainer.SetActive(true);
            lobbyUI.SetActive(true);
        }
        else if(whichOne == true)
        {
            whichOne = false;
            quitContainer.SetActive(true);
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
        lobbyContainer2.SetActive(false);
        lobbyContainer3.SetActive(false);
        lobbyUI.SetActive(false);
        lobbyUi.arrowLeft.SetActive(false);
        lobbyUi.arrowRight.SetActive(false);
        quitContainer.SetActive(false);
    }

    public void BackTolobby()
    {
        right = true;
        hideItemContainer.SetActive(true);
    }
    public void MoreContainers()
    {
        left = true;
        hideItemContainer.SetActive(true);
    }

    public void QuitBackLobby()
    {
        backToLobby = true;
    }

    public void startQuitAnim()
    {
        right = true;
        whichOne = true;
        hideItemContainer.SetActive(true);
    }
}