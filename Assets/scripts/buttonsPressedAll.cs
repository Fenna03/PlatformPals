using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class buttonsPressedAll : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;

    public Button levelButton;
    public Text MessageText;

    private void Awake()
    {
        levelButton.onClick.AddListener(() =>
        {
            Loader.loadNetwork(Loader.Scene.levelSelect);
        });
    }

    private void Start()
    {
        MessageText.text = "";
        levelButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        OnBothButtonPressServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnBothButtonPressServerRpc(ServerRpcParams serverRpcParams = default)
    {

        //Debug.Log("button 1: " + button1.gameObject.GetComponent<Animator>().GetBool("isPressed"));
        //Debug.Log("button 2: " + button2.gameObject.GetComponent<Animator>().GetBool("isPressed"));

        if (button1.gameObject.GetComponent<Animator>().GetBool("isPressed") && button2.gameObject.GetComponent<Animator>().GetBool("isPressed"))
        {
            MessageText.text = "You Finished the first level!!";
            levelButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        }
    }
}
