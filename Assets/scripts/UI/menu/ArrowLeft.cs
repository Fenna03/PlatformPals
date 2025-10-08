using System.Collections;
using UnityEngine;

public class ArrowLeft : MonoBehaviour
{
    public LevelScriptBook bookItems;
    public GameObject leftArrow;
    public GameObject rightArrow;

    private void Start()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
        StartCoroutine(WaitAndPrint());
    }

    public void OnLeftClick()
    {
        if(bookItems.playersContainer2.activeSelf == true && bookItems.playersContainer2.activeSelf == true)
        {
            bookItems.MoreContainers();
        }
        else
        {
            bookItems.MoreContainers();
            bookItems.QuitBackLobby();
        }
        //bookItems.quitOrNot = true;
    }
    public void OnRightClick()
    {
        if(bookItems.playersContainer2.activeSelf == true && bookItems.playersContainer2.activeSelf == true)
        {
            bookItems.quitOrNot = true;
            bookItems.BackTolobby();
        }
        else
        {
            bookItems.BackTolobby();
        }
    }

    private IEnumerator WaitAndPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }
    }
}