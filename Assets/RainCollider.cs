using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCollider : MonoBehaviour
{
    public Transform RainSpawner;
    private int current = 0;

    private void OnTriggerExit(Collider collider)
    {
        RainSpawner.position = new Vector3(collider.transform.position.x, 0f, collider.transform.position.z);
        RainSpawner.Translate(collider.transform.forward * 50f);

        transform.position = new Vector3(collider.transform.position.x, 50f, collider.transform.position.z);
        transform.Translate(collider.transform.forward * 50f);
    }
}
