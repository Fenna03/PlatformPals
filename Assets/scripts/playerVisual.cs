using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVisual : MonoBehaviour
{
    public void SetPlayerSkin(GameObject skin)
    {

        foreach (Transform child in this.transform)
        {
            if (child.tag == "skin")
            {
                child.gameObject.SetActive(false);
            }
        }
        Transform selectedSkin = this.gameObject.transform.Find(skin.name);
        selectedSkin.gameObject.SetActive(true);
    }
}
