using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampAnimation : MonoBehaviour
{
    private Light Light;

    void Start()
    {
        Light = GetComponent<Light>();   
    }

    void Update()
    {
        Light.intensity = Mathf.PingPong(Time.time, 0.5f) + 0.25f;
    }
}
