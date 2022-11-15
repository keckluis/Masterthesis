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
    private float BackSailDegrees;
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

        //wind direction
        //Wind.position -= new Vector3(Wind.position.x - Map.position.x, 0, Wind.position.z - Map.position.z);
        Vector3 windUpdate = Vector3.Normalize(Wind.position) - Vector3.zero;
        WindVector = new Vector2(windUpdate.x, windUpdate.z);
        WindDegrees = (Mathf.Atan2(WindVector.x, WindVector.y) * 180 / Mathf.PI);
        ShipDegrees = Map.transform.eulerAngles.y;
        //WindDegrees += ShipDegrees;
        //WindDegrees = ClampDegrees(WindDegrees);
        //float windRadians = WindDegrees * (Mathf.PI / 180);
        //WindVector = new Vector2(Mathf.RoundToInt(Mathf.Sin(windRadians)), Mathf.RoundToInt(Mathf.Cos(windRadians)));

        //rotate wind indicator according to wind direction
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        ShipSpeed = Map.velocity.magnitude;

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        ShipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));
        WindShipAngle = Vector2.Angle(WindVector, ShipVector);

        //TESTING PHYSICS VERSION
        Vector3 windForce = new Vector3(WindVector.x * WindStrength, 0, WindVector.y * WindStrength);

        BackSailCalculations(windForce);
        FrontSailCalculations(windForce);

        FrontMast.localPosition = FrontMastPos;
        FrontMast.localEulerAngles = FrontMastRot;

        ForwardForceCalculations();

        Map.AddForce(Ship.forward * ForwardForce);
    }

    private void FrontSailCalculations(Vector3 windForce)
    {
        if (WindShipAngle > 135)
        {
            WindFromFront = true;
            //FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, FrontSailPort.position);
            //FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, FrontSailStarboard.position);
        }
        else
        {
            WindFromFront = false;
            
            /*if (windFromPort)
            {
                FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce * 1.5f, FrontSailPort.position);
                FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, FrontSailStarboard.position);
            }
            else
            {
                FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, FrontSailPort.position);
                FrontSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce * 1.5f, FrontSailStarboard.position);
            }*/
        }

        /*HingeJoint frontHinge = FrontSailRing.GetComponent<HingeJoint>();
        JointLimits frontLimits = frontHinge.limits;
        float frontSheet = (WindShipAngle / 180f) * 40f;
        frontLimits.min = -frontSheet;
        frontLimits.max = frontSheet;
        frontHinge.limits = frontLimits;*/

        //rotate front sail
        //if (!WindFromFront)
        //FrontSailRing.localEulerAngles = new Vector3(0, FrontSailDegrees, 0);

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

        frontSailRotNEW = Mathf.Clamp(frontSailRotNEW, -75f, 75f);

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

        if (BackSailRing.localEulerAngles.y > 180)
        {
            BackSail.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            BackSail.localScale = new Vector3(1, 1, 1);
        }

        //rotate back sail
        //BackSailRing.localEulerAngles = new Vector3(0, BackSailDegrees, 0);

        SheetRoll2.rotation = Ship.rotation;
    }

    private void ForwardForceCalculations()
    {

        //front sail force
        if (!WindFromFront)
        {
            ForwardForce -= WindStrength * (HalyardLength / 100);
        }
        else
        {
            ForwardForce += (WindStrength * (HalyardLength / 100)) * 0.5f;
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
                ForwardForce -= WindStrength * (1 - Mathf.Abs(sailWindRatio));
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
