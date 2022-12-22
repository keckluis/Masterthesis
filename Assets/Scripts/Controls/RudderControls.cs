using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RudderControls : MonoBehaviour
{
    public Transform Rudder;
    public Transform SteeringWheel;
    public Rigidbody Ship;
    private float Speed;
    public float Degrees = 0f;

    void FixedUpdate()
    {
        Speed = Ship.velocity.magnitude;
    
        Degrees = Mathf.Clamp(Degrees, -45f, 45f);

        Rudder.localEulerAngles = new Vector3(0f, -Degrees, 0f);

        Ship.transform.Rotate(Ship.transform.up, Degrees * Speed * 0.0005f);

        if (Degrees > 0f)
            Degrees -= GetComponent<ReadMicrocontrollers>().WheelSpeed * 0.5f;
        else if (Degrees < 0f)
            Degrees += GetComponent<ReadMicrocontrollers>().WheelSpeed * 0.5f;
    }

    public void SteerTEMP(float direction)
    {
        Degrees += direction;
    }
}
