using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SailsManager : MonoBehaviour
{
    public WindDirectionType UseWindDirectionType = WindDirectionType.VECTOR;
    public Vector2 WindVector = new Vector2(1, 0);
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
    
    public float ShipDegrees;
    private float FrontSailDegrees;
    private float BackSailDegrees;
    private Vector2 ShipVector;

    public float SheetLength = 80;
    public float HalyardLength = 10;

    public bool WindFromFront = false;

    public float WindShipAngle;

    public float ForwardForce = 1;

    public float ShipSpeed = 0;

    void FixedUpdate()
    {
        //make sure ship never stops
        ForwardForce = 1;

        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 0, 80);

        //clamp halyard length (10-100%)
        HalyardLength = Mathf.Clamp(HalyardLength, 10, 100);

        FrontSail.localScale = new Vector3(1, HalyardLength * 0.01f, 1);

        FrontSailRopeRings.localPosition = new Vector3(0, FrontSail.localScale.y * -6.5f, 0);

        //wind direction
        if (UseWindDirectionType == WindDirectionType.VECTOR)
        {
            WindDegrees = (Mathf.Atan2(WindVector.x, WindVector.y) * 180 / Mathf.PI);
        }
        else
        {
            float windRadians = WindDegrees * (Mathf.PI / 180);
            WindVector = new Vector2(Mathf.RoundToInt(Mathf.Sin(windRadians)), Mathf.RoundToInt(Mathf.Cos(windRadians)));
        }

        WindDegrees = ClampDegrees(WindDegrees);

        ShipDegrees = Ship.eulerAngles.y;

        //rotate wind indicator according to wind direction
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        FrontSailCalculations();

        BackSailCalculations();

        ForwardForceCalculations();

        ShipSpeed = Ship.GetComponent<Rigidbody>().velocity.magnitude;
    }

    private void FrontSailCalculations()
    {
        //calculate front sail roation and clamp it
        FrontSailDegrees = Mathf.Clamp(WindDegrees - ShipDegrees, -40, 40);

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        ShipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));
        WindShipAngle = Vector2.Angle(WindVector, ShipVector);

        //TODO: front sail behaviour when wind comes from front
        if (WindShipAngle > 135)
        {
            WindFromFront = true;
            FrontSailDegrees = 0;
        }
        else
        {
            WindFromFront = false;
        }

        //rotate front sail
        FrontSailRing.localEulerAngles = new Vector3(0, FrontSailDegrees, 0);

        if (!WindFromFront)
        {
            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 1);
        }
        else
        {
            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 0.1f);
        }
    }

    private void BackSailCalculations()
    {
        //calculate back sail rotation and clamp to sheet length
        BackSailDegrees = WindDegrees - ShipDegrees + 180;

        if (BackSailDegrees < 0)
        {
            BackSailDegrees = 360 + BackSailDegrees;
        }

        if (BackSailDegrees > 180)
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 360 - SheetLength, 360);

            BackSail.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 0, SheetLength);

            BackSail.localScale = new Vector3(1, 1, 1);
        }

        //rotate back sail
        BackSailRing.localEulerAngles = new Vector3(0, BackSailDegrees, 0);

        SheetRoll2.rotation = Ship.rotation;
    }

    private void ForwardForceCalculations()
    {
        //front sail force
        if (!WindFromFront)
        {
            ForwardForce += WindStrength * (HalyardLength / 100);
        }
        else
        {
            ForwardForce -= (WindStrength * (HalyardLength / 100)) * 0.5f;
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
                ForwardForce += WindStrength * (1 - Mathf.Abs(sailWindRatio));
            }    
        }

        if (ForwardForce < 1)
            ForwardForce = 1;

        //add force to ship
        Ship.GetComponent<Rigidbody>().AddForce(ShipVector.x * ForwardForce, 0, ShipVector.y * ForwardForce);
    }

    private float ClampDegrees(float input)
    {
        if (input >= 360)
        {
            input -= 360;
        }
        return input;
    }
}

public enum WindDirectionType
{
    VECTOR,
    DEGREES
}
