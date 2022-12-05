using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float PulseSpeed = 100f;
    void Update()
    {
        GetComponent<Light>().intensity = Mathf.PingPong(Time.time * PulseSpeed, 20f) + 20f;
    }
}
