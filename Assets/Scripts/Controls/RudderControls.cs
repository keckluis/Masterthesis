using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RudderControls : MonoBehaviour
{
    public Transform Rudder;
    public Transform SteeringWheel;
    public Rigidbody Map;
    public Transform Wind;
    private float Speed;
    public float Degrees = 0;

    void FixedUpdate()
    {
        Speed = Map.velocity.magnitude;
        if (Input.GetKey(KeyCode.D))
        {
            Degrees -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Degrees += 1f;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (Degrees > 0)
                Degrees -= 1f;
            else if (Degrees < 0)
                Degrees += 1f;
        }

        Degrees = Mathf.Clamp(Degrees, -179f, 179f);

        SteeringWheel.localEulerAngles = new Vector3(0f, 0f, Degrees);

        float rudderDegrees = (Degrees / 179f) * 45f;

        Rudder.localEulerAngles = new Vector3(0, rudderDegrees, 0);

        Map.transform.RotateAround(Vector3.zero, transform.up, Degrees * Speed * 0.01f);
        Wind.RotateAround(Vector3.zero, transform.up, Degrees * Speed * 0.01f);
        //Map.AddTorque(new Vector3(0, Degrees * Speed * 100f, 0));
    }
}
