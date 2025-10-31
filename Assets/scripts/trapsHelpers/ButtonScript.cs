using Unity.Netcode;
using UnityEngine;

public class ButtonScript : NetworkBehaviour
{
    public Animator anim;
    public fanOnOff fanScript;
    public fireOnOff fireScript;
    public trampolineOnOff trampolineScript;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // Only clients detect local collision
        if (!IsClient) return;

        if (col.gameObject.CompareTag("Player"))
        {
            // Client tells the server a press happened
            OnButtonPressServerRpc();
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (!IsClient) return;

        if (col.gameObject.CompareTag("Player"))
        {
            OnButtonReleaseServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnButtonPressServerRpc(ServerRpcParams rpcParams = default)
    {
        PressClientRpc();
    }

<<<<<<< Updated upstream
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

=======
>>>>>>> Stashed changes
    [ServerRpc(RequireOwnership = false)]
    private void OnButtonReleaseServerRpc(ServerRpcParams rpcParams = default)
    {
        ReleaseClientRpc();
    }

    [ClientRpc]
<<<<<<< Updated upstream
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
        Debug.Log($"[ButtonScript] Collision detected on {NetworkManager.Singleton.LocalClientId}, IsServer:{IsServer}, IsClient:{IsClient}, col:{col.gameObject.name}");

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
=======
    private void PressClientRpc()
>>>>>>> Stashed changes
    {
        anim.SetBool("isPressed", true);
        anim.SetBool("isReleased", false);
        if (fanScript != null) fanScript.On();
        if (fireScript != null) fireScript.Off();
        if (trampolineScript != null) trampolineScript.On();
    }

    [ClientRpc]
    private void ReleaseClientRpc()
    {
        anim.SetBool("isPressed", false);
        anim.SetBool("isReleased", true);
        if (fanScript != null) fanScript.Off();
        if (fireScript != null) fireScript.On();
        if (trampolineScript != null) trampolineScript.Off();
    }
}