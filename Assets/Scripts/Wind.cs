using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public SailsManager SailsManager;

    void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, SailsManager.WindDegrees + 180 - SailsManager.ShipDegrees, transform.localEulerAngles.z);
    }
}
