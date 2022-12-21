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
        /*if (Input.GetKey(KeyCode.A))
        {
            Degrees -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Degrees += 1f;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (Degrees > 0f)
                Degrees -= 1f;
            else if (Degrees < 0f)
                Degrees += 1f;
        }*/

        Degrees = Mathf.Clamp(Degrees, -45f, 45f);

        Rudder.localEulerAngles = new Vector3(0f, -Degrees, 0f);

        Ship.transform.Rotate(Ship.transform.up, Degrees * Speed * 0.001f);
    }

    public void SteerTEMP(float direction)
    {
        Degrees += direction;
    }
}
