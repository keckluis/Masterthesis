using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetControls : MonoBehaviour
{
    
    public SailsManager SailsManager;

    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SailsManager.SheetLength += 0.1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            SailsManager.SheetLength -= 0.1f;
        }
    }
}
