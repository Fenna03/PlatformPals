using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class buttonAnimation : NetworkBehaviour
{
    public Animator anim;

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
        anim.SetBool("isPressed", true);
        anim.SetBool("isReleased", false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void OnButtonReleaseServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ReleaseClientRpc();
    }

    [ClientRpc]
    void ReleaseClientRpc()
    {
        anim.SetBool("isPressed", false);
        anim.SetBool("isReleased", true);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnButtonPressServerRpc();
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OnButtonReleaseServerRpc();
        }
    }

}