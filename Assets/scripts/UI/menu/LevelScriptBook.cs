using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScriptBook : MonoBehaviour
{
    public Animator anim;

    [Header("gameObjects")]
    public GameObject showItemContainer;
    public GameObject hideItemContainer;

    public GameObject playersContainer2;
    public GameObject playersContainer3;
    public GameObject playersContainer4;

    public GameObject quitContainer;

    [Header("scripts")]
    public ShowItems showItems;
    public HideItems hideItems;

    [Header("booleans")]
    public bool hasDisappeared; //if anim is done
    public bool right; //which way it flips?
    public bool left; // see above
    public bool whichOne; //is for quit button so needed.
    public bool backToLobby; //going back to normal

    //i... i need al these fucking bools? i dont know how else to do it...

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (showItemContainer != null)
        {
            hideItemContainer.SetActive(false);
            showItemContainer.SetActive(false);

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
        else if (hasDisappeared == true && right == true)
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

        showItemContainer.SetActive(true);
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
           // lobbyContainer.SetActive(true);
           // lobbyUI.SetActive(true);
        }
        else if (whichOne == true)
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
        playersContainer2.SetActive(false);
        playersContainer3.SetActive(false);
        playersContainer4.SetActive(false);
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
