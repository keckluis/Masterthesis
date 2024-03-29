using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class WindStrings : MonoBehaviour
{
    [SerializeField] private Transform[] Positions;
    [SerializeField] private float WiggleAmount = 0.1f;
    [SerializeField] private float WiggleSpeed = 0.5f;
    [SerializeField] private float StringLength = 7.5f;
    private LineRenderer LR;

    private void Start()
    {
        LR = GetComponent<LineRenderer>();  
    }

    void FixedUpdate()
    {
        //float yScale = Mathf.Abs((1 /transform.parent.parent.localScale.x) * 0.02f);
        //transform.parent.localScale = new Vector3(0.2f, yScale, 0.2f);

        Vector3[] positions = new Vector3[Positions.Length];

        float wiggle = WiggleAmount - Mathf.PingPong(Time.time * WiggleSpeed, WiggleAmount * 2f);

        float length = -(StringLength / 3f);
        Positions[1].localPosition = new Vector3(-wiggle, 0, length * 1);
        Positions[2].localPosition = new Vector3(+wiggle, 0, length * 2);
        Positions[3].localPosition = new Vector3(wiggle/2, 0, length * 3);

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = Positions[i].localPosition;   
        }

        LR.SetPositions(positions);
    }
}
