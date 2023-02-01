using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindStringsPosition : MonoBehaviour
{
    [SerializeField] private Transform BackSail;
    [SerializeField]  private float StartPosX = -0.13f;

    void Update()
    {
        transform.localPosition = new Vector3(StartPosX * BackSail.localScale.x, 0f, 0f);
    }
}
