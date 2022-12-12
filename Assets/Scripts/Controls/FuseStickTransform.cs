using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseStickTransform : MonoBehaviour
{
    public Transform XROrigin;
    void LateUpdate()
    {
        transform.position -= XROrigin.position;
    }
}
