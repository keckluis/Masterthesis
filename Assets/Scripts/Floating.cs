using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float DepthBeforeSubmerged = 1;
    public float DisplacementAmount = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < 1)
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / DepthBeforeSubmerged) * DisplacementAmount;
            Rigidbody.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
        }           
    }
}
