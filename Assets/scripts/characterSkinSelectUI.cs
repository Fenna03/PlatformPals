using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class characterSkinSelectUI : MonoBehaviour
{
    [SerializeField] private int skinId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;

    //private void Awake()
    //{
    //    GetComponent<Button>().onClick.AddListener(() => {
    //        optionsScript.Instance.ChangePlayerSkin(skinId);
    //    });
    //}
}
