using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    //private Rigidbody Ship;
    //public float DepthBeforeSubmerged = 1;
    //public float DisplacementAmount = 3;

    private void Start()
    {
        // Ship = GetComponent<Rigidbody>();   
    }

    void FixedUpdate()
    {
       /* if (transform.position.y < 0.5f)
        {
            float displacementMultiplier = Mathf.Clamp01(-transform.position.y / DepthBeforeSubmerged) * DisplacementAmount;
            Ship.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
        }*/        

        float y = Mathf.PingPong(Time.time * 0.25f, 0.2f);
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
