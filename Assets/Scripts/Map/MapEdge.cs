using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdge : MonoBehaviour
{
    private void Update()
    {
        float x = transform.position.x;
        if (transform.position.x > 2900f)
        {
            x = -2850f;
        }
        else if (transform.position.x < -2900f)
        {
            x = 2850f;
        }

        float z = transform.position.z;
        if (transform.position.z > 2900f)
        {
            z = -2850f;
        }
        else if (transform.position.z < -2900f)
        {
            z = 2850f;
        }

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
