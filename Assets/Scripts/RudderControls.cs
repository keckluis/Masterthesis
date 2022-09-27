using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RudderControls : MonoBehaviour
{
    public Transform Rudder;
    public Transform SteeringWheel;
    public Rigidbody Ship;
    public Transform Rear;
    
    private float Speed;

    void FixedUpdate()
    {
        Speed = Ship.velocity.magnitude * 0.5f;
        if (Input.GetKey(KeyCode.D))
        {
            //Ship.Rotate(0, -Speed * 0.1f, 0);
            Ship.AddForceAtPosition(-transform.right * Speed * 0.01f, Rear.position);
            SteeringWheel.Rotate(0, 0, -1);

            Rudder.Rotate(0, -1, 0);
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Ship.Rotate(0, Speed * 0.1f, 0);
            Ship.AddForceAtPosition(transform.right * Speed * 0.01f, Rear.position);
            SteeringWheel.Rotate(0, 0, 1);

            Rudder.Rotate(0, 1, 0);
        }

        float rudderCap = Rudder.localEulerAngles.y;

        if (rudderCap > 45 && rudderCap < 180)
            rudderCap = 45;
        else if (rudderCap < 315 && rudderCap > 180)
            rudderCap = 315;

        Rudder.localEulerAngles = new Vector3(Rudder.localEulerAngles.x, rudderCap, Rudder.localEulerAngles.z);

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            SteeringWheel.localEulerAngles = Vector3.zero;
            Rudder.localEulerAngles = Vector3.zero;
        }  
    }
}
