using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RunBoyRun : MonoBehaviour
{
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if(transform.position.x > 10)
        {
            Destroy(gameObject);
        }
    }
}
