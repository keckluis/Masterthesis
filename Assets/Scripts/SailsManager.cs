using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SailsManager : MonoBehaviour
{
    public Vector2 WindDirection = new Vector2(1, 0);
    public float WindStrength = 1;
    public Transform Ship;
    public Transform WindIndicator;
    public Transform FrontSailRing;
    public Transform FrontSail;
    public Transform FrontSailRopeRings;
    public Transform BackSailRing;
    public Transform BackSail;
    public Transform SheetRoll2;
    
    public float WindDegrees;
    public float ShipDegrees;
    public float FrontSailDegrees;
    public float BackSailDegrees;

    public float SheetLength = 80;
    public int HalyardLength = 10;

    public float ForwardForce = 1;

    private bool WindFromFront = false;
    public float WindShipAngle;
    private Vector2 ShipVector;

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

        //wind and ship rotation
        WindDegrees = (Mathf.Atan2(WindDirection.x, WindDirection.y) * 180 / Mathf.PI);
        ShipDegrees = Ship.eulerAngles.y;

        WindDegrees = ClampDegrees(WindDegrees);
        
        //rotate wind indicator according to wind direction
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        FrontSailCalculations();

        BackSailCalculations();

        ForwardForceCalculations();
    }

    private void FrontSailCalculations()
    {
        //calculate front sail roation and clamp it
        FrontSailDegrees = Mathf.Clamp(WindDegrees - ShipDegrees, -40, 40);

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        ShipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));
        WindShipAngle = Vector2.Angle(WindDirection, ShipVector);

        //TODO: front sail behaviour when wind comes from front
        if (ShipDegrees > 180)
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
            ForwardForce += WindStrength * ((float)HalyardLength / 100);
        }
            
        //back sail force
        float backSailAngle = BackSailDegrees;
        if (backSailAngle > 180)
        {
            backSailAngle = 360 - backSailAngle;
        }

        if (WindShipAngle < 150)
        {

            //value = 0 -> ideal sail/wind angle
            float sailWindRatio = 2.25f - Mathf.Abs(WindShipAngle / backSailAngle);
            //print(sailWindRatio);
            if (sailWindRatio < 1 && sailWindRatio > -1)
            {
                ForwardForce += WindStrength * (1 - Mathf.Abs(sailWindRatio));
            }    
        }

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
