using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    Animator Animator;
    public Transform Camera;
    public float RotationSpeed = 1;

    void Start()
    {
        Animator = GetComponent<Animator>();   
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            Animator.SetBool("isWalking", true);
        else
            Animator.SetBool("isWalking", false);

        if (Input.GetKey(KeyCode.A))
            Camera.Rotate(new Vector3(0, -RotationSpeed, 0));
        if (Input.GetKey(KeyCode.D))
            Camera.Rotate(new Vector3(0, RotationSpeed, 0));
    }
}
