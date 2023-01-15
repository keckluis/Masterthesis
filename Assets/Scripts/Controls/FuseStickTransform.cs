using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseStickTransform : MonoBehaviour
{
    [SerializeField] private Transform XROrigin;
    void LateUpdate()
    {
        transform.position -= XROrigin.position;
    }
}
