using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ButtonScript : NetworkBehaviour
{
    public Animator anim;
    public fanOnOff fanScript;
    public fireOnOff fireScript;
    public trampolineOnOff trampolineScript;

    //public BoxCollider2D BC;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnButtonPressServerRpc(ServerRpcParams serverRpcParams = default)
    {
        PressClientRpc();
    }

    [ClientRpc]
    void PressClientRpc()
    {
        Debug.Log($"PressClientRpc called on {NetworkManager.Singleton.LocalClientId}");
        anim.SetBool("isPressed", true);
        anim.SetBool("isReleased", false);
        if(fanScript != null)
        {
            fanScript.On();
        }
        if (fireScript != null)
        {
            fireScript.Off();
        }
        if (trampolineScript != null)
        {
            trampolineScript.On();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnButtonReleaseServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ReleaseClientRpc();
    }

    [ClientRpc]
    void ReleaseClientRpc()
    {
        //Debug.Log("test");
        anim.SetBool("isPressed", false);
        anim.SetBool("isReleased", true);
        if (fanScript != null)
        {
            fanScript.Off();
        }
        if (fireScript != null)
        {
            fireScript.On();
        }
        if (trampolineScript != null)
        {
            trampolineScript.Off();
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!IsClient) return; // only detect on client

        if (col.gameObject.CompareTag("Player"))
        {
            if (optionsScript.Instance.isOnline)
            {
                OnButtonPressServerRpc();
            }
            else
            {
                PressLocal();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (optionsScript.Instance.isOnline == true)
            {
                OnButtonReleaseServerRpc();
            }
            else
            {
                ReleaseLocal();
            }
        }
    }

    private void PressLocal()
    {
        anim.SetBool("isPressed", true);
        anim.SetBool("isReleased", false);

        if (fanScript != null)
        {
            fanScript.On();
        }
        if (fireScript != null)
        {
            fireScript.Off();
        }
        if (trampolineScript != null)
        {
            trampolineScript.On();
        }
    }
    private void ReleaseLocal()
    {
        anim.SetBool("isPressed", false);
        anim.SetBool("isReleased", true);

        if (fanScript != null)
        {
            fanScript.Off();
        }
        if (fireScript != null)
        {
            fireScript.On();
        }
        if (trampolineScript != null)
        {
            trampolineScript.Off();
        }
    }
}