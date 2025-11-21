using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MovingPlayer : NetworkBehaviour
{
    private Vector2 moveInput;

    private float speed = 4.5f;
    public float health = 10f;
    public float jumpingPower = 7f;

    public bool isGrounded = false;
    private bool isFacingRight = true;

    public int jumpAmount = 1;

    private Rigidbody2D rb;
    public Animator anim;

    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // ⛔ Do NOT apply input logic in menu / lobby / level-select scenes
        if (!IsGameplayScene())
            return;

        // ⛔ Only the owner gets input
        if (!IsOwner)
            return;

        if (GameManager.Instance.isOnline)
        {
            SetupOnlineInput();
        }
        else if (GameManager.Instance.isLocal)
        {
            if (playerInput != null)
                playerInput.enabled = true;
        }

        InitAnimations();
    }
    private bool IsGameplayScene()
    {
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // TODO: Replace these with your real level names
        return scene.StartsWith("Level_") || scene == "GameScene";
    }

    private void SetupOnlineInput()
    {
        // Remove any existing user safely
        foreach (var user in InputUser.all)
        {
            if (user.valid)
                user.UnpairDevicesAndRemoveUser();
        }

        var newUser = InputUser.CreateUserWithoutPairedDevices();

        InputUser.PerformPairingWithDevice(Keyboard.current, newUser);
        InputUser.PerformPairingWithDevice(Mouse.current, newUser);

        newUser.AssociateActionsWithUser(playerInput.actions);
    }

    private void InitAnimations()
    {
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
    }


    private void Update()
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        Flip();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        if (context.performed && isGrounded)
        {
            JumpPlayer();
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.isOnline)
        {
            if (context.performed)
                GameManager.Instance.TogglePauseGame();
        }
        else if (GameManager.Instance.isLocal)
        {
            if (context.performed)
                LocalGameManager.Instance.TogglePause();
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

        if (GameManager.Instance.isOnline)
            SendAnimationClientRpc(true, jumpAmount >= 1);
    }

    private void MovePlayer()
    {
        if (rb == null) return;

        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        bool isRunning = Mathf.Abs(moveInput.x) > 0.01f;
        if (isGrounded)
        {
            anim.SetBool("isRunning", isRunning);
        }

        if (GameManager.Instance.isOnline)
            SendAnimationClientRpc(isRunning, anim.GetBool("isFalling"));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        if (collision.collider.CompareTag("ground") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("button"))
        {
            jumpAmount = 0;
            isGrounded = true;

            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);

            if (GameManager.Instance.isOnline)
                SendAnimationClientRpc(anim.GetBool("isRunning"), false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (GameManager.Instance.isOnline && !IsOwner)
            return;

        if (collision.collider.CompareTag("ground") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("button"))
        {
            isGrounded = false;
            anim.SetBool("isFalling", true);
            anim.SetBool("isRunning", false);

            if (GameManager.Instance.isOnline)
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