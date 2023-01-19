using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBoomRingAudio : MonoBehaviour
{
    private float Prev;
    private AudioSource AS;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Mathf.Abs(transform.localEulerAngles.y - Prev) > 0.1f)
        {
            AS.enabled = true;
            Prev = transform.localEulerAngles.y;
        }
        else
        {
            AS.enabled = false;
        }
    }
}
