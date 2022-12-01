using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetColliderTEMP : MonoBehaviour
{
    bool Left = false;
    bool Right = false;
    public SailsManager SailsManager;
    private void FixedUpdate()
    {
        if ((Left && Right) || (!Left && !Right))
        {
            return;
        }

        if (Left && !Right)
        {
            SailsManager.SheetLength -= 0.1f;
        }
        else if (!Left && Right)
        {
            SailsManager.SheetLength += 0.1f;
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
