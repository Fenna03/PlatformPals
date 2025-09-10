using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScriptBook : MonoBehaviour
{
    public Animator anim;

    [Header("gameObjects")]
    //to make the items appear and disappear
    public GameObject showItemContainer;
    public GameObject hideItemContainer;

    //container levels
    public GameObject playersContainer2;
    public GameObject playersContainer3;
    public GameObject playersContainer4;

    public GameObject quitContainer;

    [Header("scripts")]
    public ShowItems showItems;
    public HideItems1 hideItems;

    [Header("booleans")]
    public bool hasDisappeared; //if anim is done
    public bool right; //which way it flips?
    public bool left; // see above
    public bool quitOrNot; //is for quit button so needed.
    public bool backToPlayer23; //going back to normal

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
            playersContainer2.SetActive(false);
            playersContainer3.SetActive(false);
            playersContainer4.SetActive(false);
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

    //end
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
        if (quitOrNot == false && backToPlayer23 == false)
        {
            playersContainer4.SetActive(true);
        }
        else if (backToPlayer23 == true)
        {
            //this just goes back to normal lobby which is player 2/3 intro levels for me
            activateButtons();
            backToPlayer23 = false;
        }
        anim.SetBool("flipLeft", false);

        showItemContainer.SetActive(true);
        left = false;
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
        activateButtons();
        right = false;
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

        if (quitOrNot == false)
        {
            //so if it's not quit you go back to the normal one that you also see while opening
            playersContainer2.SetActive(true);
            playersContainer3.SetActive(true);
        }
        else if (quitOrNot == true)
        {
            //here it is quit and it will go to quit
            quitOrNot = false;
            quitContainer.SetActive(true);
        }
    }

    public void HideItems()
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

    //so these 3 are called so the boolean is true to know which has to be active
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
        backToPlayer23 = true;
    }

    //this one activates the quit
    public void startQuitAnim()
    {
        right = true;
        quitOrNot = true;
        hideItemContainer.SetActive(true);
    }
}
