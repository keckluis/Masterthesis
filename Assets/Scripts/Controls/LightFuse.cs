using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFuse : MonoBehaviour
{
    [SerializeField] private Cannon Cannon;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fuse")
        {
            Debug.Log(Cannon.gameObject.name + " fired!");
            Cannon.Shoot();
        }
    }
}
