using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdgeCollider : MonoBehaviour
{
    bool ShipAtEdge = false;
    public Transform Wind;
    private float Speed;
    private void FixedUpdate()
    {
        if (ShipAtEdge)
        {
            Speed = GetComponent<Rigidbody>().velocity.magnitude;
            transform.RotateAround(Vector3.zero, transform.up, 150f * Speed * 0.001f);
            Wind.RotateAround(Vector3.zero, transform.up, 150f * Speed * 0.001f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            ShipAtEdge= true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            ShipAtEdge= false;
        }
    }
}
