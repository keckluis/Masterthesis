using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRope : MonoBehaviour
{
    public Transform StartPoint;
    public List<Transform> MiddlePoints;
    public Transform EndPoint;
    private LineRenderer LR;

    void Awake()
    {
        LR = GetComponent<LineRenderer>();
        LR.enabled = true;
        LR.useWorldSpace = true;
        LR.positionCount = MiddlePoints.Count + 2;
    }

    void Update()
    {
        Vector3[] positions = new Vector3[MiddlePoints.Count + 2];
        positions[0] = StartPoint.position;

        int i = 1;
        foreach(Transform mp in MiddlePoints)
        {
            positions[i] = mp.position;
            i++;
        }

        positions[MiddlePoints.Count + 1] = EndPoint.position;
        LR.SetPositions(positions);
    }
}
