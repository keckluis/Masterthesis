using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            print("HIT: " + transform.parent.gameObject.name); 
            Destroy(collision.gameObject);
        }
    }
}
