using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAcross : MonoBehaviour
{
    private float speed = 5f;
    private Vector3 startPos = new Vector3(-10f, -3.32f, 0f);
    private bool isStopped = false;

    public GameObject objectToSpawn; // Assign a prefab in the inspector
    public List<GameObject> randomPlayer;

    Animator anim;

    void Start()
    {
        StartCoroutine(WaitforRandomAmount());
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= 0f && isStopped == false)
        {
            if (Random.value < 0.1f)
            {
                Debug.Log("Stopping at center...");
                isStopped = true; // PREVENT multiple coroutines
                StartCoroutine(StopAtZero());
            }
        }

        if (transform.position.x >= 10)
        {
            transform.position = startPos;
            StartCoroutine(WaitforRandomAmount());
        }
    }

    private IEnumerator WaitforRandomAmount()
    {
        speed = 0;
        float value = Random.Range(20, 100);
        Debug.Log(value);
        yield return new WaitForSeconds(value);
        speed = 5;
    }

    private IEnumerator StopAtZero()
    {
        isStopped = true;
        speed = 0f;
        anim.SetBool("isIdle", true);

        //which player is going to run with you?
        int number = Random.Range(0, randomPlayer.Count);
        objectToSpawn = randomPlayer[number];

        // Spawn something at current position
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, new Vector3(-10, -3.32f, 0), Quaternion.identity);
        }

        // Wait a random time
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        anim.SetBool("isIdle", false);
        speed = 5f;
        isStopped = false;
    }
}