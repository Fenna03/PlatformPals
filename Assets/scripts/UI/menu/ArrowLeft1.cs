using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowLeft1 : MonoBehaviour
{
    public BookItems bookItems;
    public void OnLeftClick()
    {
        Debug.Log("left");
        bookItems.MoreContainers();
        bookItems.whichOne = true;
    }
    public void OnRightClick()
    {
        Debug.Log("right");
        bookItems.BackTolobby();
    }
}