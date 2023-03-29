using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RudderControls : MonoBehaviour
{
    [SerializeField] private Transform Rudder;
    public Transform SteeringWheel;
    [SerializeField] private Rigidbody Ship;
    public float Degrees = 0f;
    [SerializeField] private Transform RotationPoint;

    void FixedUpdate()
    {
        Degrees = Mathf.Clamp(Degrees, -45f, 45f);

        Rudder.localEulerAngles = new Vector3(0f, -Degrees, 0f);

        Ship.transform.RotateAround(RotationPoint.position, Ship.transform.up, Degrees * 0.0005f);

        if (Degrees > 0f)
            Degrees -= GetComponent<ReadMicrocontrollers>().WheelSpeed * 0.5f;
        else if (Degrees < 0f)
            Degrees += GetComponent<ReadMicrocontrollers>().WheelSpeed * 0.5f;
    }
}
