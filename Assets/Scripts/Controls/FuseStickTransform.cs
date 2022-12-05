using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseStickTransform : MonoBehaviour
{
    public Transform Tracker;

    void Update()
    {
        transform.position = Tracker.position;
        transform.localEulerAngles = Tracker.localEulerAngles;
    }
}
