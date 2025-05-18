using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSideways : MonoBehaviour
{
    public float speed = 1f;
    private bool isMovingLeft = true;

    public float leftBound = -71.6f;
    public float rightBound = -71.4f;

    void Update()
    {
        if (isMovingLeft)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x <= leftBound)
            {
                isMovingLeft = false;
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            if (transform.position.x >= rightBound)
            {
                isMovingLeft = true;
            }
        }
    }
}

