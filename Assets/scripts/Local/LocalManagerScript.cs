using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalManagerScript : MonoBehaviour
{
    public GameObject exclamation;
    public List<GameObject> playerSkinList;

    // Start is called before the first frame update
    void Start()
    {
        DisableImage();
    }
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

    public void EnableImage()
    {
        exclamation.SetActive(true);
    }
    public void DisableImage()
    {
        exclamation.SetActive(false);
    }
}
