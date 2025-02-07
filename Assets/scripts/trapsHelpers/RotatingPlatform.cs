using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 50f; // Degrees per second

    void Update()
    {
        // Rotate around the Y axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
