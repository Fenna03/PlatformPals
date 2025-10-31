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

    [ServerRpc(RequireOwnership = false)]
    private void OnButtonReleaseServerRpc(ServerRpcParams rpcParams = default)
    {
        ReleaseClientRpc();
    }

    [ClientRpc]
    private void PressClientRpc()
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
