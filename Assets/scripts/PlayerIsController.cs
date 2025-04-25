using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIsController : MonoBehaviour
{
    public GameObject virtualMouse;
    public bool isInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if(isInstantiated == false)
            {
                Instantiate(virtualMouse);
                isInstantiated = true;
            }
        }
    }
}
