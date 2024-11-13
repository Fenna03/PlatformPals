using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawOnOff : MonoBehaviour
{
    private float dirX;
    public float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = 1f;
        moveSpeed = 8f; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("STOP"))
        {
            dirX *= -1f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2 (dirX * moveSpeed, rb.velocity.y);
    }

    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if(dirX > 0)
        {
            facingRight = false;
        }
        else if(dirX < 0)
        {
            facingRight = true;
        }

        if(((facingRight) && (localScale.x <0)) || ((!facingRight) && localScale.x > 0))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }
}
