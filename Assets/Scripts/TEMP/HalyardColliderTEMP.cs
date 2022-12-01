using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalyardColliderTEMP : MonoBehaviour
{
    bool Left = false;
    bool Right = false;
    public SailsManager SailsManager;

    void Start()
    {
        InvokeRepeating("ChangeHalyard", 0.0f, 0.05f);
    }

    void ChangeHalyard()
    {
        if ((Left && Right) || (!Left && !Right))
        {
            return;
        }

        if (Left && !Right)
        {
            SailsManager.HalyardLength -= 1;
        }
        else if (!Left && Right)
        {
            SailsManager.HalyardLength += 1;
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
            Right = true;
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
