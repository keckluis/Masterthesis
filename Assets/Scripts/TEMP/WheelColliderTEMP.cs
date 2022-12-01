using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelColliderTEMP : MonoBehaviour
{
    bool Left = false;
    bool Right = false;
    public RudderControls RC;

    private void FixedUpdate()
    {
        if ((Left && Right) || (!Left && !Right))
        {
            if (RC.Degrees > 0)
                RC.Degrees -= 1f;
            else if (RC.Degrees < 0)
                RC.Degrees += 1f;
        }
        else if (Left && !Right)
        {
            RC.Degrees += 1f;
        }
        else if (!Left && Right)
        {
            RC.Degrees -= 1f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "LeftController")
        {
            Left = true;
        }

        if (collision.gameObject.tag == "RightController")
        {
            Right= true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "LeftController")
        {
            Left = false;
        }

        if (collision.gameObject.tag == "RightController")
        {
            Right = false;
        }
    }
}
