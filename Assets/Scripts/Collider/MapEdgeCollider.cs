using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdgeCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
        }
    }
}
