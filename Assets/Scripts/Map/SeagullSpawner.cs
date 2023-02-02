using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
    [SerializeField] private int Height = 40;
    [SerializeField] private float Speed = 0.1f;

    private Vector3 From;

    private void Start()
    {
        int axis = Random.Range(0, 4);

        if (axis == 0)
        {
            From = new Vector3(-3000f, Height, Random.Range(-3000f, 3000f));
            transform.localEulerAngles = new Vector3(0f, Random.Range(45f, 135f));

        }
        else if (axis == 1)
        {
            From = new Vector3(3000f, Height, Random.Range(-3000f, 3000f));
            transform.localEulerAngles = new Vector3(0f, Random.Range(225f, 315f));
        }
        else if (axis == 2)
        {
            From = new Vector3(Random.Range(-3000f, 3000f), Height, 3000f);
            transform.localEulerAngles = new Vector3(0f, Random.Range(135f, 235f));
        }
        else if (axis == 3)
        {
            From = new Vector3(Random.Range(-3000f, 3000f), Height, -3000f);
            transform.localEulerAngles = new Vector3(0f, Random.Range(-45f, 45f));
        }

        transform.position = From;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Speed, Space.Self);

        if (Mathf.Abs(transform.position.x) > 3000f || Mathf.Abs(transform.position.z) > 3000f)
        {
            Start();
        }
    }
}
