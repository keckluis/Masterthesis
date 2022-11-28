using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetColliders : MonoBehaviour
{ 
    public Transform Ship;

    void Update()
    {
        transform.position = Ship.position;
        transform.rotation = Ship.rotation;
    }
}
