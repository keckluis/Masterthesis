using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalyardControls : MonoBehaviour
{
    public SailsManager SailsManager;

    void Start()
    {
        InvokeRepeating("ChangeHalyard", 0.0f, 0.05f);
    }

    void ChangeHalyard()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            SailsManager.HalyardLength -= 1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            SailsManager.HalyardLength += 1f;
        }
    }
}
