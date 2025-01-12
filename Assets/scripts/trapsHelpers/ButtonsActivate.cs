using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ButtonsActivate : NetworkBehaviour
{
    public Animator anim;
    public bool youGoGirl;

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
        anim.SetBool("isPressed", true);
        anim.SetBool("isReleased", false);
        youGoGirl = true;
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
        youGoGirl = false;
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Debug.Log("player touches");
            OnButtonPressServerRpc();
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Debug.Log("player stop touches");
            OnButtonReleaseServerRpc();
        }
    }
}
