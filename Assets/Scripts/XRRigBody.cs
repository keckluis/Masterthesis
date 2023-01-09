using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRRigBody : MonoBehaviour
{
    public Transform Body;
    
    void Update()
    {
        Body.position = transform.position;
        Body.Translate(0f, -1.5f, 0f);

        Body.eulerAngles = Vector3.RotateTowards(Body.eulerAngles, new Vector3(0f, transform.eulerAngles.y, 0f), Time.deltaTime, 1f);
    }
}
