using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerVisual : MonoBehaviour
{

    public void SetPlayerSkin(GameObject skin)
    {
        Transform selectedSkin = this.gameObject.transform.Find(skin.name);
        selectedSkin.gameObject.SetActive(true);
    }
}
