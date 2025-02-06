using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MovingPlayer : NetworkBehaviour
{
    private Vector2 moveInput;

    private float speed = 4.5f;
    public float health = 10f;

    //jumping
    public float jumpingPower = 7f;
    public bool isGrounded = true;
    public int jumpAmount = 0;

    //flipping
    private bool isFacingRight = true;

    //things on character
    Rigidbody2D rb;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        //Moving();
        Flip();
    }

    private void FixedUpdate()
    {
        // Apply the movement in FixedUpdate for consistent physics updates
        MovePlayer();
    }
    // Called by Unity Input System when "Move" action is triggered
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        
    }

    // Called by Unity Input System when "Jump" action is triggered
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            JumpPlayer();
        }
    }

    private void JumpPlayer()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpAmount++;
            isGrounded = false;
            anim.SetBool("isJumping", true);
            if (jumpAmount >= 1)
            {
                anim.SetBool("isFalling", true);
            }
        }
    }

    private void MovePlayer()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground" || collision.collider.tag == "Player" || collision.collider.tag == "button")
        {
            jumpAmount = 0;
            isGrounded = true;
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "ground" || collision.collider.tag == "Player" || collision.collider.tag == "button")
        {
            isGrounded = false;
            anim.SetBool("isFalling", true);
            anim.SetBool("isRunning", false);
        }
    }

    void Flip()
    {
        if (isFacingRight && moveInput.x < 0f || !isFacingRight && moveInput.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            anim.SetBool("isRunning", true);
        }
        else if(moveInput.x == 0f)
        {
            anim.SetBool("isRunning", false);
        }
    }
}



//void Moving()
//{
//    if (Input.GetKey(KeyCode.A))
//    {
//        transform.Translate(Vector3.left * speed * Time.deltaTime);
//        horizontal = -1f;
//        anim.SetBool("isRunning", true);
//    }
//    else if (Input.GetKey(KeyCode.D))
//    {
//        transform.Translate(Vector3.right * speed * Time.deltaTime);
//        horizontal = 1f;
//        anim.SetBool("isRunning", true);
//    }
//    else
//    {
//        anim.SetBool("isRunning", false);
//    }
//    if (Input.GetKey(KeyCode.W) && jumpAmount < 1 || Input.GetKey(KeyCode.Space) && jumpAmount < 1)
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
//        jumpAmount++;
//        isGrounded = false;
//        anim.SetBool("isJumping", true);
//        if (jumpAmount >= 1)
//        {
//            anim.SetBool("isFalling", true);
//        }
//    }
//}