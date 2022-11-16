using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public Transform Hand;
    public Transform Cube;

    void LateUpdate()
    {
        Hand.position = Cube.position;
    }
}
