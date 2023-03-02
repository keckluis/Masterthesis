using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustXROrigin : MonoBehaviour
{
    float Speed = 0.01f;

    void Start()
    {
        if (PlayerPrefs.HasKey("x") && PlayerPrefs.HasKey("z"))
        {
            float x = PlayerPrefs.GetFloat("x");
            float z = PlayerPrefs.GetFloat("z");
            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        }
            
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetFloat("x", transform.localPosition.x);
            PlayerPrefs.SetFloat("z", transform.localPosition.z);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow)) 
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Speed, transform.localPosition.y, transform.localPosition.z);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localPosition = new Vector3(transform.localPosition.x - Speed, transform.localPosition.y, transform.localPosition.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + Speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - Speed);
        }
    }
}
