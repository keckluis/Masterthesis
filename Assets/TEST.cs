using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public Transform Forward;

    // Update is called once per frame
    void Update()
    {
        transform.position = Forward.position;
        transform.rotation = Forward.rotation;

        transform.Translate(Vector3.forward * 300f);
    }
}
