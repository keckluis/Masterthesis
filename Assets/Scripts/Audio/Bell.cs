using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    AudioSource AS;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (AS.isPlaying)
        {
            transform.localEulerAngles = new Vector3(Mathf.PingPong(Time.time * 100f, 60f) - 30f, 0f, 0f);
        }
    }
}
