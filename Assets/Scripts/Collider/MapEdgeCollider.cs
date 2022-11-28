using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdgeCollider : MonoBehaviour
{
    //bool ShipAtEdge = false;

    Transform CollisionShip;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            //ShipAtEdge = true;
            CollisionShip = collision.transform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            //ShipAtEdge = false;
            CollisionShip = null;
        }
    }

    private void FixedUpdate()
    {
        if (CollisionShip != null)
        {
            CollisionShip.Rotate(Vector3.up, .1f);
        }
    }
}
