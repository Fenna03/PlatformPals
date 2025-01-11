using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sawOnOff : MonoBehaviour
{
    private float dirX;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale; 
    
    GameObject respawner;

    public List<MovingPlayer> dying = new List<MovingPlayer>();

    // Start is called before the first frame update
    void Start()
    {
        respawner = GameObject.Find("Respawner");
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = 1f;
        moveSpeed = 5f; 
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dying.Add(collision.gameObject.GetComponent<MovingPlayer>());
            if (dying.Count > 0)
            {
                foreach (MovingPlayer player in dying)
                {
                    player.transform.position = respawner.transform.position;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        dying.Remove(collision.gameObject.GetComponent<MovingPlayer>());
    }
}
