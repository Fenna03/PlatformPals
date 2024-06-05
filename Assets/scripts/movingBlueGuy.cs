using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movingBlueGuy : Photon.MonoBehaviour
{
    public float speed = 3f;

    //jumping
    public float horizontal;
    public float jumpingPower = 7f;
    public bool isGrounded = true;
    public int jumpAmount = 0;

    //flipping
    private bool isFacingRight = true;

    //things on character
    Rigidbody2D rb;
    public new BoxCollider2D collider;
    public Animator anim;

    //scripts
    //public buttonDiff buttonScript;
    //public GameObject button;

    public GameObject playerCamera;
    public Text playerNameText;
    public PhotonView photonView;

    private void Awake()
    {
        if (photonView.isMine)
        {
            playerCamera.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //buttonScript = GetComponent<buttonDiff>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (photonView.isMine)
        {
            Moving();
        }

        if (isGrounded == false)
        {
            anim.SetBool("isRunning", false);
        }

        //Flip();
    }

    private void Moving()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            horizontal = -1f;
            photonView.RPC("FlipCheck", PhotonTargets.AllBuffered, horizontal);
            anim.SetBool("isRunning", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            horizontal = 1f;
            photonView.RPC("FlipCheck", PhotonTargets.AllBuffered, horizontal);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.W) && jumpAmount < 1)
        {
            // transform.Translate(Vector3.up * speed * Time.deltaTime);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpAmount++;
            isGrounded = false;
            anim.SetBool("isJumping", true);
            anim.SetBool("isRunning", false);
            if (jumpAmount >= 1)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isFalling", true);
            }
        }
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground" || collision.collider.tag == "Player" || collision.collider.tag == "button")
        {
            jumpAmount = 0;
            isGrounded = true;
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.collider.tag == "ground" || collision.collider.tag == "Player" || collision.collider.tag == "button")
        {
            isGrounded = false;
            anim.SetBool("isRunning", false);
        }
    }

    [PunRPC]
    private void FlipCheck(float direction)
    {
        if (isFacingRight && direction < 0f || !isFacingRight && direction > 0f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
