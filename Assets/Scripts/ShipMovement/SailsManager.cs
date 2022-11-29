using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SailsManager : MonoBehaviour
{
    public Vector2 WindVector = new Vector2(1, 0);
    public Transform Wind;
    public float WindDegrees;
    public float WindStrength = 1;
    public Transform Ship;
    public Transform WindIndicator;
    public Transform FrontSailRing;
    public Transform FrontSail;
    public Transform FrontSailRopeRings;
    public Transform BackSailRing;
    public Transform BackSail;
    public Transform SheetRoll2;
    public Transform FrontSailPort;
    public Transform FrontSailStarboard;
    public Transform FrontMast;

    public Rigidbody Map;
    
    public float ShipDegrees;
    [SerializeField]private float BackSailDegrees;
    public Vector2 ShipVector;

    public float SheetLength = 80;
    public float HalyardLength = 10;

    public bool WindFromFront = false;

    public float WindShipAngle;

    public float ForwardForce = -1;

    public float ShipSpeed = 0;

    private Vector3 FrontMastPos;
    private Vector3 FrontMastRot;

    public Transform WindString;

    bool FrontSailSwitch = false;

    private void Start()
    {
        FrontMastPos = FrontMast.localPosition;
        FrontMastRot = FrontMast.localEulerAngles;
    }

    void FixedUpdate()
    {
        //make sure ship never stops
        ForwardForce = -1;

        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 1, 80);

        //clamp halyard length (10-100%)
        HalyardLength = Mathf.Clamp(HalyardLength, 10, 100);

        
        Vector3 windUpdate = Vector3.Normalize(Wind.position) - Vector3.zero;
        WindVector = new Vector2(windUpdate.x, windUpdate.z);
        WindDegrees = (Mathf.Atan2(WindVector.x, WindVector.y) * 180 / Mathf.PI);
        ShipDegrees = Map.transform.eulerAngles.y;
       
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        ShipSpeed = Map.velocity.magnitude;
        WindShipAngle = Vector2.Angle(WindVector, new Vector2(Ship.forward.x, Ship.forward.z));

        Vector3 windForce = new Vector3(WindVector.x * WindStrength, 0, WindVector.y * WindStrength);

        BackSailCalculations(windForce);
        FrontSailCalculations(windForce);

        FrontMast.localPosition = FrontMastPos;
        FrontMast.localEulerAngles = FrontMastRot;

        ForwardForceCalculations();

        Map.AddForce(Ship.forward * (ForwardForce * WindStrength));
    }

    private void FrontSailCalculations(Vector3 windForce)
    {
        if (WindShipAngle > 135)
        {
            WindFromFront = true;
            
        }
        else
        {
            WindFromFront = false;
            
        }

        float backSailRot = BackSailRing.localEulerAngles.y;

        if (backSailRot > 180)
            backSailRot -= 360;

        float frontSailRot = FrontSailRing.localEulerAngles.y;
        float frontSailRotNEW = 80f - Mathf.Abs(backSailRot);

        if (frontSailRot > 180)
            frontSailRot -= 360;

        if (backSailRot < 0 && frontSailRot < 0)
        {
            FrontSailSwitch = true;
            frontSailRotNEW = frontSailRot + 1f;
        }
        else if (backSailRot > 0 && frontSailRot > 0)
        {
            FrontSailSwitch = true;
            frontSailRotNEW = frontSailRot - 1f;
        }
        else
        {
            FrontSailSwitch = false;
        }

        if (!FrontSailSwitch)
        {
            if (backSailRot > 0)
                frontSailRotNEW = -frontSailRotNEW;
        }

        frontSailRotNEW = Mathf.Clamp(frontSailRotNEW, -60f, 60f);

        if (frontSailRot < frontSailRotNEW + 1f && frontSailRot > frontSailRotNEW - 1f)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot, 0f);
        else if (frontSailRot > frontSailRotNEW)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot - 1f, 0f);
        else if (frontSailRot < frontSailRotNEW)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot + 1f, 0f);
        

        FrontSail.localScale = new Vector3(1, HalyardLength * 0.01f, 1);
        FrontSailRopeRings.localPosition = new Vector3(0, FrontSail.localScale.y * -6.5f, 0);

        if (!WindFromFront)
        {
            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 1);
        }
        else
        {
            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 0.1f);
        }
    }

    private void BackSailCalculations(Vector3 windForce)
    {  
        BackSailRing.GetComponent<Rigidbody>().AddForce(windForce);
        HingeJoint backHinge = BackSailRing.GetComponent<HingeJoint>();
        JointLimits backLimits = backHinge.limits;
        backLimits.min = -SheetLength;
        backLimits.max = SheetLength;
        backHinge.limits = backLimits;

        //calculate back sail rotation and clamp to sheet length
        BackSailDegrees = WindDegrees - ShipDegrees + 180;

        if (BackSailDegrees < 0)
        {
            BackSailDegrees = 360 + BackSailDegrees;
        }

        if (BackSailDegrees > 180)
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 360 - SheetLength, 360);
        }
        else
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 0, SheetLength);
        }

        float sailCurvature = (359 - BackSailDegrees) / 79;
        sailCurvature = Mathf.Clamp(sailCurvature, 0.1f, 1f);
        
        if (BackSailRing.localEulerAngles.y > 180)
        {
            BackSail.localScale = new Vector3(-sailCurvature, 1, 1);
        }
        else
        {
            BackSail.localScale = new Vector3(sailCurvature, 1, 1);
        }

        SheetRoll2.rotation = Ship.rotation;
    }

    private void ForwardForceCalculations()
    {

        //front sail force
        if (!WindFromFront)
        {
            ForwardForce -= (HalyardLength / 100);
        }
        else
        {
            ForwardForce += (HalyardLength / 100) * 0.5f;
        }
            
        //back sail force
        float backSailAngle = BackSailDegrees;
        if (backSailAngle > 180)
        {
            backSailAngle = 360 - backSailAngle;
        }

        if (WindShipAngle < 170)
        {
            //value = 0 -> ideal sail/wind angle
            float sailWindRatio = 2.25f - Mathf.Abs((180 - WindShipAngle) / backSailAngle);

            if (sailWindRatio < 1 && sailWindRatio > -1)
            {
                ForwardForce -= 1 - Mathf.Abs(sailWindRatio);
                WindString.localEulerAngles = new Vector3(0, sailWindRatio * 45f, 0);
            }
            else
            {
                if (sailWindRatio > 0)
                    WindString.localEulerAngles = new Vector3(0, +45f, 0);
                else
                    WindString.localEulerAngles = new Vector3(0, -45f, 0);
            }         
        }

        if (ForwardForce > -1)
            ForwardForce = -1;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, 17, 0), new Vector3(WindVector.x * 3, 17, WindVector.y * 3));
        Gizmos.DrawSphere(new Vector3(WindVector.x * 3, 17, WindVector.y * 3), 0.1f);
    }
}
