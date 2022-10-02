using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public Rigidbody Ship;
    public float DepthBeforeSubmerged = 1;
    public float DisplacementAmount = 3;

    void FixedUpdate()
    {
        if (transform.position.y < 1)
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / DepthBeforeSubmerged) * DisplacementAmount;
            Ship.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
        }           
    }
}
