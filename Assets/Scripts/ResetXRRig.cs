using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetXRRig : MonoBehaviour
{
    public Transform ResetPosition;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.localPosition = ResetPosition.localPosition;
        }
    }
}
