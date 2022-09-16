using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    public Vector2 Speed = new Vector2(5, 5);
    private float RotationX = 0.0f;
    private Camera Camera;

    void Start()
    {
        Camera = GetComponent<Camera>();    
    }

    void FixedUpdate()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Speed.y;

        RotationX -= mouseY;
        RotationX = Mathf.Clamp(RotationX, -90, 90);

        Camera.transform.localEulerAngles = new Vector3(RotationX, 180.0f, 0.0f);
    }
}
