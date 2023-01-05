using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SailsManager : MonoBehaviour
{
    public Vector2 WindVector = new Vector2(1f, 0f);
    //public Transform Wind;
    public float WindDegrees;
    public float WindStrength = 1;
    public Rigidbody Ship;
    public Transform WindIndicator;
    public Transform FrontSailRing, FrontSail, FrontSailRopeRings;

    public Transform BackSailRing, BackSail;
    public Transform SheetRoll2;
    public Transform FrontSailPort, FrontSailStarboard;
    public Transform FrontMast;

    //public Rigidbody Map;
    
    public float ShipDegrees;
    public float BackSailDegrees;
    public Vector2 ShipVector;

    public float SheetLength = 80f;
    public float HalyardLength = 10f;

    public bool WindFromFront = false;

    public float WindShipAngle;

    public float ForwardForce = -1f;

    public float ShipSpeed = 0f;

    private Vector3 FrontMastPos, FrontMastRot;

    public Transform WindString;

    bool FrontSailSwitch = false;

    public Transform Port, BackBoom, Starboard;
    public bool SailOnPort = false;

    float backSailRingPrev = 0f;

    private void Start()
    {
        FrontMastPos = FrontMast.localPosition;
        FrontMastRot = FrontMast.localEulerAngles;
    }

    private void Update()
    {
        float port = Vector3.Magnitude(Port.position - BackBoom.position);
        float starboard = Vector3.Magnitude(Starboard.position - BackBoom.position);

        if (port < starboard)
        {
            SailOnPort= true;
        }
        else
        {
            SailOnPort= false;
        }

        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 1f, 80f);

        //clamp halyard length (10-100%)
        HalyardLength = Mathf.Clamp(HalyardLength, 10f, 100f);

        WindDegrees = (Mathf.Atan2(WindVector.x, WindVector.y) * 180f / Mathf.PI);
        ShipDegrees = Ship.transform.eulerAngles.y;

        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        ShipSpeed = Ship.velocity.magnitude;
        WindShipAngle = Vector2.Angle(WindVector, new Vector2(Ship.transform.forward.x, Ship.transform.forward.z));

        Vector3 windForce = new Vector3(WindVector.x * WindStrength, 0f, WindVector.y * WindStrength);

        BackSailCalculations(windForce);
        FrontSailCalculations(windForce);

        FrontMast.localPosition = FrontMastPos;
        FrontMast.localEulerAngles = FrontMastRot;

        ForwardForceCalculations();
        Ship.AddForce(Ship.transform.forward * (ForwardForce * WindStrength));
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

        if (backSailRot > 180f)
            backSailRot -= 360f;

        float frontSailRot = FrontSailRing.localEulerAngles.y;
        float frontSailRotNEW = 80f - Mathf.Abs(backSailRot);

        if (frontSailRot > 180f)
            frontSailRot -= 360f;

        if (backSailRot < 0f && frontSailRot < 0f)
        {
            FrontSailSwitch = true;
            frontSailRotNEW = frontSailRot + 1f;
        }
        else if (backSailRot > 0f && frontSailRot > 0f)
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
            if (backSailRot > 0f)
                frontSailRotNEW = -frontSailRotNEW;
        }

        frontSailRotNEW = Mathf.Clamp(frontSailRotNEW, -60f, 60f);

        if (frontSailRot < frontSailRotNEW + 1f && frontSailRot > frontSailRotNEW - 1f)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot, 0f);
        else if (frontSailRot > frontSailRotNEW)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot - 1f, 0f);
        else if (frontSailRot < frontSailRotNEW)
            FrontSailRing.localEulerAngles = new Vector3(0f, frontSailRot + 1f, 0f);
        

        FrontSail.localScale = new Vector3(1f, HalyardLength * 0.01f, 1f);
        FrontSailRopeRings.localPosition = new Vector3(0f, FrontSail.localScale.y * -6.5f, 0f);

        if (!WindFromFront)
        {
            FrontSail.localScale = new Vector3(1f, FrontSail.localScale.y, 1f);
        }
        else
        {
            FrontSail.localScale = new Vector3(1f, FrontSail.localScale.y, 0.1f);
        }
    }

    private void BackSailCalculations(Vector3 windForce)
    {  
        BackSailRing.GetComponent<Rigidbody>().AddForce(windForce);
        Ship.AddForce(-windForce);
        HingeJoint backHinge = BackSailRing.GetComponent<HingeJoint>();
        JointLimits backLimits = backHinge.limits;
        backLimits.min = -SheetLength;
        backLimits.max = SheetLength;
        backHinge.limits = backLimits;

        //calculate back sail rotation and clamp to sheet length
        BackSailDegrees = WindDegrees - ShipDegrees + 180f;

        if (BackSailDegrees < 0f)
        {
            BackSailDegrees = 360f + BackSailDegrees;
        }

        if (BackSailDegrees > 180f)
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 360f - SheetLength, 360f);
        }
        else
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 0f, SheetLength);
        }

        float sailCurvature = (359f - BackSailDegrees) / 79f;
        sailCurvature = Mathf.Clamp(sailCurvature, 0.1f, 1f);
        
        if (BackSailRing.localEulerAngles.y > 180)
        {
            BackSail.localScale = new Vector3(-sailCurvature, 1f, 1f);
        }
        else
        {
            BackSail.localScale = new Vector3(sailCurvature, 1f, 1f);
        }

        SheetRoll2.rotation = Ship.rotation;

        if (Mathf.Abs(BackSailRing.localEulerAngles.y - backSailRingPrev) > 0.1f)
        {
            BackSailRing.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            BackSailRing.GetComponent<AudioSource>().enabled = true;
        }
        backSailRingPrev = BackSailRing.localEulerAngles.y;
    }

    private void ForwardForceCalculations()
    {
        //reset force before new calculation 
        ForwardForce = 1f;

        //front sail force
        if (!WindFromFront)
        {
            ForwardForce += (HalyardLength / 100f);
        }
        else
        {
            ForwardForce -= (HalyardLength / 100f) * 0.5f;
        }
            
        //back sail force
        float backSailAngle = BackSailDegrees;
        if (backSailAngle > 180f)
        {
            backSailAngle = 360f - backSailAngle;
        }

        if (WindShipAngle < 170f)
        {
            //value = 0 -> ideal sail/wind angle
            float sailWindRatio = 2.25f - Mathf.Abs((180f - WindShipAngle) / backSailAngle);

            if (sailWindRatio < 1f && sailWindRatio > -1f)
            {
                ForwardForce += 1f - Mathf.Abs(sailWindRatio);
                WindString.localEulerAngles = new Vector3(0f, sailWindRatio * 45f, 0f);
            }
            else
            {
                if (sailWindRatio > 0f)
                    WindString.localEulerAngles = new Vector3(0f, +45f, 0f);
                else
                    WindString.localEulerAngles = new Vector3(0f, -45f, 0f);
            }         
        }

        //make sure ship never stops
        if (ForwardForce < 1f)
            ForwardForce = 1f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0f, 17f, 0f), new Vector3(WindVector.x * 3f, 17f, WindVector.y * 3f));
        Gizmos.DrawSphere(new Vector3(WindVector.x * 3f, 17f, WindVector.y * 3f), 0.1f);
    }
}
