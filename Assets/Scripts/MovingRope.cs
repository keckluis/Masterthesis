using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRope : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    private LineRenderer LR;

    void Awake()
    {
        LR = GetComponent<LineRenderer>();
        LR.enabled = true;
        LR.useWorldSpace = true;
    }

    void Update()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = StartPoint.position;
        positions[1] = EndPoint.position;
        LR.SetPositions(positions);
    }
}
