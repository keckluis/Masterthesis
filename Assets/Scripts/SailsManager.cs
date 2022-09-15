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
    public Transform FrontSail;
    public Transform BackSail;

    public float WindDegrees;
    public float ShipDegrees;
    public float FrontSailDegrees;
    public float BackSailDegrees;

    public float SheetLength = 80;

    public float ForwardForce = 1;

    [SerializeField]
    private bool WindFromFront = false;

    void FixedUpdate()
    {
        ForwardForce = 1;
        //clamp sheet length
        SheetLength = Mathf.Clamp(SheetLength, 0, 80);

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
        FrontSail.localEulerAngles = new Vector3(FrontSail.localEulerAngles.x, FrontSail.localEulerAngles.y, FrontSailDegrees);

        //calculate back sail rotation and clamp to sheet length
        BackSailDegrees = WindDegrees - ShipDegrees;

        if (BackSailDegrees < 0)
            BackSailDegrees = 360 + BackSailDegrees;

        if (BackSailDegrees > 180)
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 360 - SheetLength, 360);
        else
            BackSailDegrees = Mathf.Clamp(BackSailDegrees, 0, SheetLength);

        //rotate back sail
        BackSail.localEulerAngles = new Vector3(-90, BackSailDegrees, 0);

        if (!WindFromFront)
        {
            float frontSailForce = FrontSailDegrees;
            if (frontSailForce > 180)
                frontSailForce = 360 - frontSailForce;

            frontSailForce = (50 - frontSailForce) / 5;
            ForwardForce += frontSailForce;
        }

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        Vector2 shipVector = new Vector2(-Mathf.Sin(shipRadians), -Mathf.Cos(shipRadians));
        Ship.GetComponent<Rigidbody>().AddForce(shipVector.x * ForwardForce, 0, shipVector.y * ForwardForce);


        //TODO: back sail force (angle between wind and ship -> ideal sheet length)
        print(Vector2.Angle(WindDirection, shipVector));
    }
}
