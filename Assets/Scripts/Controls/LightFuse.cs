using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFuse : MonoBehaviour
{
    [SerializeField] private Cannon Cannon;

    /*void OnCollisionEnter(Collision collision)
    {
        print("AAAAAA");
        Debug.Log(Cannon.gameObject.name + " fired!");
        Cannon.Shoot();
        
    }*/

    void OnTriggerEnter(Collider collider)
    {
        print("AAAAAA");
        Debug.Log(Cannon.gameObject.name + " fired!");
        Cannon.Shoot();
        
    }
}
