using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MovingRope : MonoBehaviour
{
    public Transform StartPoint;
    public Transform MiddlePoint1, MiddlePoint2;
    public Transform EndPoint;
    private LineRenderer LR;
    public SailsManager SailsManager;

    void Awake()
    {
        LR = GetComponent<LineRenderer>();
        LR.enabled = true;
        LR.useWorldSpace = true;

        if (MiddlePoint1 != null && MiddlePoint2 != null)
            LR.positionCount = 4;
        else
            LR.positionCount = 2;
    }

    void Update()
    {
        Vector3[] positions = new Vector3[4];
        positions[0] = StartPoint.position;

        Vector3 startToEnd = EndPoint.position - StartPoint.position;

        if (MiddlePoint1 != null && MiddlePoint2 != null) 
        { 
            MiddlePoint1.position = StartPoint.position + startToEnd * (1f/3f);
            MiddlePoint2.position = StartPoint.position + startToEnd * (2f/3f);

            float sailDegrees = SailsManager.BackSailRing.localEulerAngles.y;
            if (sailDegrees > 180f)
                sailDegrees = 360f - sailDegrees;
            sailDegrees = Mathf.Abs(sailDegrees);
            float sheetLength = SailsManager.SheetLength;
            
            if (sailDegrees < sheetLength - 1f)
            {
                float ratio = 1 - (sailDegrees / sheetLength); 

                if (!SailsManager.SailOnPort)
                {
                    MiddlePoint1.position += new Vector3(.5f * ratio, -.5f * ratio, 0f);
                    MiddlePoint2.position += new Vector3(.5f * ratio, -.5f * ratio, 0f);
                }
                else
                {
                    MiddlePoint1.position += new Vector3(-.5f * ratio, -.5f * ratio, 0f);
                    MiddlePoint2.position += new Vector3(-.5f * ratio, -.5f * ratio, 0f);
                }

            }
            

            positions[1] = MiddlePoint1.position;
            positions[2] = MiddlePoint2.position;
            positions[3] = EndPoint.position;
        }
        else
            positions[1] = EndPoint.position;

        LR.SetPositions(positions);
    }
}
