using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public int Direction = 0;
    public float Strength = 1;
    public Transform Ship;
    public Transform WindIndicator;
    public Transform FrontBoomRing;

    void FixedUpdate()
    {
        //keep wind direction between 0 and 359 degrees
        if (Direction > 180)
            Direction= 180;
        else if (Direction < -180)
            Direction = -180;

        //show wind direction on ship
        WindIndicator.eulerAngles = new Vector3(WindIndicator.eulerAngles.x, Direction + 180, WindIndicator.eulerAngles.z);

        float frontBoomRingDirection = Mathf.Clamp(Direction, -45, 45);
        float rotX = FrontBoomRing.localEulerAngles.x;
        float rotY = FrontBoomRing.localEulerAngles.y;

        if (WindIndicator.eulerAngles.y > 80 && WindIndicator.eulerAngles.y < 280)
        {
            float force = -(Strength - (frontBoomRingDirection / 20));
            Ship.GetComponent<Rigidbody>().AddForce(0, 0, force);
            FrontBoomRing.localEulerAngles = new Vector3(rotX, rotY, frontBoomRingDirection);
        } 
    }
}
