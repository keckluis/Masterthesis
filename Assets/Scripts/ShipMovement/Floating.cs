using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private float startY;
    [SerializeField] private float FloatingAmount;
    [SerializeField] private float FloatingSpeed;

    private void Start()
    {
        startY = transform.localPosition.y;
    }

    void FixedUpdate()
    {        
        float y = Mathf.PingPong(Time.time * FloatingSpeed, FloatingAmount) + startY - (FloatingAmount / 2f);
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
}
