using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetXRRig : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(1.5f, transform.position.y, -5.5f);
        }
    }
}
