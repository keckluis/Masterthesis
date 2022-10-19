using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Floating : MonoBehaviour
{
    //private Rigidbody Ship;
    //public float DepthBeforeSubmerged = 1;
    //public float DisplacementAmount = 3;
    float startY;

    private void Start()
    {
        // Ship = GetComponent<Rigidbody>();   
        startY = transform.localPosition.y;
    }

    void FixedUpdate()
    {
        /* if (transform.position.y < 0.5f)
         {
             float displacementMultiplier = Mathf.Clamp01(-transform.position.y / DepthBeforeSubmerged) * DisplacementAmount;
             Ship.AddForce(new Vector3(0, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0), ForceMode.Acceleration);
         }*/        

        float y = Mathf.PingPong(Time.time * 0.25f, 0.2f) + startY - 0.1f;
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
