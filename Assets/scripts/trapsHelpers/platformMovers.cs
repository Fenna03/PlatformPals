using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class platformMovers : MonoBehaviour
{
    private float dirX;
    public float moveSpeed = 1f;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;

    public List<ButtonsActivate> ButtonsActivate;
    //public ButtonsActivate buttons;

    public List<MovingPlayer> players = new List<MovingPlayer>();

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = 1f;
        moveSpeed = 3f;
    }

    private void Update()
    {
        var components = FindObjectsOfType<MovingPlayer>();
        foreach (var component in components)
        {
            // Check if the component is already in the list
            if (!players.Contains(component))
            {
                // If not, add it to the list
                players.Add(component);
            }
        }
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
        bool shouldMove = false;

        foreach (ButtonsActivate buttons in ButtonsActivate)
        {
            if (buttons.youGoGirl)
            {
                shouldMove = true;
                break;  // No need to check further; movement condition met.
            }
        }

        if (shouldMove)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);  // Stop horizontal movement.
        }
    }


    private void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
        {
            facingRight = false;
        }
        else if (dirX < 0)
        {
            facingRight = true;
        }

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && localScale.x > 0))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }
}
