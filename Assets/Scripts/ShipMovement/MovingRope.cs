using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MovingRope : MonoBehaviour
{
    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform MiddlePoint1, MiddlePoint2;
    [SerializeField] private Transform EndPoint;
    private LineRenderer LR;
    [SerializeField] private SailsManager SailsManager;
    [SerializeField] private Transform ClientBackSailRing;
    public float ClientSheetLength;

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

            float sailDegrees;
            float sheetLength;
            if (SailsManager!= null) 
            {
                sailDegrees = SailsManager.BackSailRing.localEulerAngles.y;
                
                sheetLength = SailsManager.SheetLength;
            }
            else
            {
                sailDegrees = ClientBackSailRing.localEulerAngles.y;
                sheetLength = ClientSheetLength;
            }

            if (sailDegrees > 180f)
                sailDegrees = 360f - sailDegrees;
            sailDegrees = Mathf.Abs(sailDegrees);

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
