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
    public Transform BackSailRing;

    public float WindDegrees;
    public float ShipDegrees;
    public float FrontSailDegrees;
    public float BackSailDegrees;

    public float SheetLength = 80;
    public int HalyardLength = 10;

    public float ForwardForce = 1;

    private bool WindFromFront = false;

    private Vector3 FrontSailScale;

    private void Start()
    {
        FrontSailScale = FrontSail.localScale;
    }

    void FixedUpdate()
    {
        //make sure ship never stops
        ForwardForce = 1;

        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 0, 80);

        //clamp halyard length (10-100%)
        HalyardLength = Mathf.Clamp(HalyardLength, 10, 100);

        FrontSail.localScale = new Vector3(FrontSailScale.x, FrontSailScale.y, FrontSailScale.z * HalyardLength / 100);

        //wind and ship rotation
        WindDegrees = (Mathf.Atan2(WindDirection.x, WindDirection.y) * 180 / Mathf.PI) + 180;
        ShipDegrees = Ship.eulerAngles.y;

        if (WindDegrees >= 360)
            WindDegrees -= 360;
        
        //rotate wind indicator according to wind direction
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, WindDegrees, WindIndicator.eulerAngles.z);

        //calculate front sail roation and clamp it
        FrontSailDegrees = Mathf.Clamp(WindDegrees - 180 - ShipDegrees, -45, 45);

        //TODO: front sail behaviour when wind comes from front
        if (ShipDegrees > 180)
        {
            WindFromFront = true;
            FrontSailDegrees = 0;
        }
        else
            WindFromFront = false;

        if (FrontSailDegrees < 0)
            FrontSailDegrees = 360 + FrontSailDegrees;

        //rotate front sail
        FrontSailRing.localEulerAngles = new Vector3(FrontSailRing.localEulerAngles.x, FrontSailRing.localEulerAngles.y, FrontSailDegrees);

        //calculate back sail rotation and clamp to sheet length
        BackSailDegrees = WindDegrees - ShipDegrees;

        if (BackSailDegrees < 0)
            BackSailDegrees = 360 + BackSailDegrees;

        if (BackSailDegrees > 180)
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 360 - SheetLength, 360);
        else
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 0, SheetLength);

        //rotate back sail
        BackSailRing.localEulerAngles = new Vector3(-90, BackSailDegrees, 0);

        if (!WindFromFront)
        {
            float frontSailForce = FrontSailDegrees;
            if (frontSailForce > 180)
                frontSailForce = 360 - frontSailForce;

            frontSailForce = (50 - frontSailForce) / 5;
            ForwardForce += (frontSailForce * HalyardLength / 100) * WindStrength;
        }
        else
        {
            float frontSailFactor = HalyardLength;
            ForwardForce -= ((frontSailFactor / 100) - 0.1f) * WindStrength;
        }

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        Vector2 shipVector = new Vector2(-Mathf.Sin(shipRadians), -Mathf.Cos(shipRadians));
        
        float windShipAngle = Vector2.Angle(WindDirection, shipVector);

        float backSailAngle = BackSailDegrees;
        if (backSailAngle > 180)
            backSailAngle = 360 - backSailAngle;

        if (windShipAngle > 30)
            ForwardForce += (10 * WindStrength) - (Mathf.Abs((windShipAngle / 180) - (backSailAngle / 80)) * 10);

        if (ForwardForce < 0)
            ForwardForce = 1;

        //add force to ship
        Ship.GetComponent<Rigidbody>().AddForce(shipVector.x * ForwardForce, 0, shipVector.y * ForwardForce);
    }
}
