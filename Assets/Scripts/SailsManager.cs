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
    public Transform FrontSailRings;
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

    void FixedUpdate()
    {
        //make sure ship never stops
        ForwardForce = 1;

        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 0, 80);

        //clamp halyard length (10-100%)
        HalyardLength = Mathf.Clamp(HalyardLength, 10, 100);

        FrontSail.localScale = new Vector3(1, HalyardLength * 0.01f, 1);

        //wind and ship rotation
        WindDegrees = (Mathf.Atan2(WindDirection.x, WindDirection.y) * 180 / Mathf.PI);
        ShipDegrees = Ship.eulerAngles.y;

        if (WindDegrees >= 360)
        {
            WindDegrees -= 360;
        }
            
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
            float frontSailForce = FrontSailDegrees;
            if (frontSailForce > 180)
            {
                frontSailForce = 360 - frontSailForce;
            }

            frontSailForce = (50 - frontSailForce) / 5;
            ForwardForce += (frontSailForce * HalyardLength / 100) * WindStrength;

            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 1);
        }
        else
        {
            float frontSailFactor = HalyardLength;
            ForwardForce -= ((frontSailFactor / 100) - 0.1f) * WindStrength;

            FrontSail.localScale = new Vector3(1, FrontSail.localScale.y, 0.1f);
        }

        if (FrontSailRings.localScale.y != 1 + FrontSail.localScale.y && FrontSail.localScale.y < 1)
            FrontSailRings.localScale = new Vector3(1, 1 / FrontSail.localScale.y, FrontSailRings.localScale.z);

        if (FrontSailRings.localScale.z != 1 + FrontSail.localScale.z && FrontSail.localScale.z < 1)
            FrontSailRings.localScale = new Vector3(1, FrontSailRings.localScale.y, 1 / FrontSail.localScale.z);
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
        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        Vector2 shipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));

        float windShipAngle = Vector2.Angle(WindDirection, shipVector);

        float backSailAngle = BackSailDegrees;
        if (backSailAngle > 180)
        {
            backSailAngle = 360 - backSailAngle;
        }

        if (windShipAngle > 30)
        {
            ForwardForce += (10 * WindStrength) - (Mathf.Abs((windShipAngle / 180) - (backSailAngle / 80)) * 10);
        }

        if (ForwardForce < 1)
        {
            ForwardForce = 1;
        }

        //add force to ship
        Ship.GetComponent<Rigidbody>().AddForce(shipVector.x * ForwardForce, 0, shipVector.y * ForwardForce);
    }
}
