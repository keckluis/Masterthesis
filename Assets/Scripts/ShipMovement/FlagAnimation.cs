using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagAnimation : MonoBehaviour
{
    void Update()
    {
        transform.localScale = new Vector3(1 - Mathf.PingPong(5 * Time.time, 2), 1, 1);
    }
}
