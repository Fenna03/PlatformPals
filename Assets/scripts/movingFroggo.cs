using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingFroggo : MonoBehaviour
{
    public float speed = 3f;

    //jumping
    public float horizontal;
    public float jumpingPower = 7f;
    public bool isGrounded = true;
    public int jumpAmount = 0;

    //flipping
    private bool isFacingRight = true;

    Rigidbody2D rb;
    public new BoxCollider2D collider;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            horizontal = -1f;
            anim.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            horizontal = 1f;
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.UpArrow) && jumpAmount < 1)
        {
            // transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpAmount++;
            isGrounded = false;
            anim.SetBool("isJumping", true);
            //anim.SetBool("isRunning", false);
            if (jumpAmount >= 1)
            {
                //anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", true);
            }
        }

        Flip();
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            jumpAmount = 0;
            isGrounded = true;
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            isGrounded = false;
            anim.SetBool("isRunning", false);
        }
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
