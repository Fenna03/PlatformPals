using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSlowly : MonoBehaviour
{
    public float speed;
    public float resetX;
    public GameObject prefab;

    private static Vector3 spawnOffset;
    private bool hasSpawned = false;

    void Start()
    {
        // Calculate total bounds from all SpriteRenderers in children
        Bounds totalBounds = new Bounds(transform.position, Vector3.zero);
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length > 0)
        {
            totalBounds = renderers[0].bounds;
            foreach (var sr in renderers)
            {
                totalBounds.Encapsulate(sr.bounds);
            }
        }

        spawnOffset = new Vector3(totalBounds.size.x, 0f, 0f);
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > resetX && !hasSpawned)
        {
            Debug.Log("test");
            hasSpawned = true;

            // Spawn the clone just to the left of this object
            Vector3 spawnPosition = transform.position - spawnOffset;
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
        else if(transform.position.x > 55)
        {
            Destroy(gameObject);
        }
    }
}

//{
//    public Vector3 startPos;
//    public Vector3 endPos;
//    public float speed;

//    // Start is called before the first frame update
//    void Start()
//    {
//        startPos = new Vector3(0, 0, 0);
//        endPos = new Vector3(100, 0, 0);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);
//        if (transform.position.x >= 100)
//        {
//            Debug.Log("test");
//        }
//    }
//}