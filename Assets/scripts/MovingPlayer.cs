using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPlayer : NetworkBehaviour
{
    private Vector2 moveInput;

    [Header("Stats")]
    private float speed = 4.5f;
    public float health = 10f;

    [Header("Jumping")]
    public float jumpingPower = 7f;
    public bool isGrounded = true;
    public int jumpAmount = 0;

    [Header("Flip & Components")]
    private bool isFacingRight = true;
    private Rigidbody2D rb;
    public Animator anim;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var input = GetComponent<PlayerInput>();
        if (input != null)
            input.enabled = IsOwner; // enable input only for the local owner

        Debug.Log($"{gameObject.name}: PlayerInput enabled = {input.enabled} (Owner: {IsOwner})");

        // Initialize animations
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
    }

    private void Update()
    {
        if (!IsOwner) return;

        // Only owner handles flipping locally
        Flip();
    }

    private void FixedUpdate()
    {
        if (IsOwner)
            MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        if (context.performed && isGrounded)
        {
            JumpPlayer();
        }
    }

    private void JumpPlayer()
    {
        if (rb == null) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        jumpAmount++;
        isGrounded = false;

        anim.SetBool("isJumping", true);
        if (jumpAmount >= 1)
            anim.SetBool("isFalling", true);

        // Sync animations to others
        SendAnimationClientRpc(true, jumpAmount >= 1);
    }

    private void MovePlayer()
    {
        if (rb == null) return;

        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        // Sync running animation
        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f;
        if(isGrounded == true)
        {
            anim.SetBool("isRunning", isRunning);
        }
        SendAnimationClientRpc(isRunning, anim.GetBool("isFalling"));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsOwner) return;

        if (collision.collider.CompareTag("ground") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("button"))
        {
            jumpAmount = 0;
            isGrounded = true;

            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);

            SendAnimationClientRpc(anim.GetBool("isRunning"), false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsOwner) return;

        if (collision.collider.CompareTag("ground") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("button"))
        {
            isGrounded = false;
            anim.SetBool("isFalling", true);
            anim.SetBool("isRunning", false);

            SendAnimationClientRpc(false, true);
        }
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput.x < 0f) || (!isFacingRight && moveInput.x > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    [ClientRpc]
    private void SendAnimationClientRpc(bool isRunning, bool isFalling)
    {
        if (IsOwner) return; // skip local owner

        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isFalling", isFalling);
    }
}
