using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFuse : MonoBehaviour
{
    public Cannon Cannon;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fuse")
        {
            Debug.Log(Cannon.gameObject.name + " fired!");
            Cannon.Shoot();
        }
    }
}
