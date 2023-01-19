using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotAudio : MonoBehaviour
{
    AudioSource AS;
    bool waiting = false;

    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!waiting)
        {
            waiting = true;
            AS.Play();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(10, 25));
        waiting = false;
    }
}
