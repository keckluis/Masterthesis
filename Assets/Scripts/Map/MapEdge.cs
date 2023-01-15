using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdge : MonoBehaviour
{
    [SerializeField] private Transform Ship;
    private void Update()
    {
        float x = Ship.transform.position.x;
        if (Ship.transform.position.x > 2900f)
        {
            x = -2850f;
        }
        else if (Ship.transform.position.x < -2900f)
        {
            x = 2850f;
        }

        float z = Ship.transform.position.z;
        if (Ship.transform.position.z > 2900f)
        {
            z = -2850f;
        }
        else if (Ship.transform.position.z < -2900f)
        {
            z = 2850f;
        }

        Ship.transform.position = new Vector3(x, Ship.transform.position.y, z);
    }
}
