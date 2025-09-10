using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowLeft : MonoBehaviour
{
    public LevelScriptBook bookItems;
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
}