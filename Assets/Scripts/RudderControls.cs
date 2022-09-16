using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RudderControls : MonoBehaviour
{
    public Transform Rudder;
    public Transform SteeringWheel;
    public Transform Ship;
    private float Speed;

    private Vector3 WheelStartRotation;
    private Vector3 RudderStartRotation;

    private void Start()
    {
        WheelStartRotation = SteeringWheel.localEulerAngles;
        RudderStartRotation = Rudder.localEulerAngles;
    }

    void FixedUpdate()
    {
        Speed = Ship.GetComponent<Rigidbody>().velocity.magnitude * 0.5f;
        if (Input.GetKey(KeyCode.A))
        {
            Ship.Rotate(0, -Speed * 0.1f, 0);
            SteeringWheel.Rotate(0, 1, 0);

            Rudder.Rotate(0, 0, 1);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            Ship.Rotate(0, Speed * 0.1f, 0);
            SteeringWheel.Rotate(0, -1, 0);

            Rudder.Rotate(0, 0, -1);
        }

        float rudderCap = Rudder.localEulerAngles.y;

        if (rudderCap > 45 && rudderCap < 180)
            rudderCap = 45;
        else if (rudderCap < 315 && rudderCap > 180)
            rudderCap = 315;

        Rudder.localEulerAngles = new Vector3(Rudder.localEulerAngles.x, rudderCap, Rudder.localEulerAngles.z);

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            SteeringWheel.localEulerAngles = WheelStartRotation;
            Rudder.localEulerAngles = RudderStartRotation;
        }  
    }
}
