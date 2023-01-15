using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCollider : MonoBehaviour
{
    [SerializeField] private Transform Ship;
    private void OnTriggerExit()
    {        
        transform.position = new Vector3(Ship.position.x, 0, Ship.position.z);
        transform.Translate(Ship.forward * 50f);
    }
}
