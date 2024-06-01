using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBlueGuy : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            horizontal = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            horizontal = 1f;
        }
        if (Input.GetKey(KeyCode.W) && jumpAmount < 1)
        {
           // transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpAmount++;
            isGrounded = false;
        }

        Flip();
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            jumpAmount = 0;
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground")
        {
            isGrounded = false;
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
            Debug.Log("flip");
        }
     }
}
