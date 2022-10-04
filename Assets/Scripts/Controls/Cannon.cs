using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform Horizontal;
    public Transform Vertical;
    public Transform CanonBallsHolder;
    public GameObject CanonBall;
    public Collider Ship;

    //public ParticleSystem Fire;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            RotateHorizontal(-0.1f);
        if (Input.GetKey(KeyCode.RightArrow))
            RotateHorizontal(0.1f);
        if (Input.GetKey(KeyCode.UpArrow))
            RotateVertical(-0.1f);
        if (Input.GetKey(KeyCode.DownArrow))
            RotateVertical(0.1f);

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void RotateHorizontal(float direction)
    {
        Horizontal.Rotate(new Vector3(0, direction, 0));
    }

    void RotateVertical(float direction)
    {
        Vertical.Rotate(new Vector3(direction, 0, 0));

        if (Vertical.localEulerAngles.x > 25 && Vertical.localEulerAngles.x < 180)
        {
            Vertical.localEulerAngles = new Vector3(25, 0, 0);
        }
        else if (Vertical.localEulerAngles.x < 335 && Vertical.localEulerAngles.x > 180)
        {
           Vertical.localEulerAngles = new Vector3(335, 0, 0);
        }
    }   

    void Shoot()
    {
        GameObject cb = Instantiate(CanonBall, CanonBallsHolder);
        cb.transform.position = Vertical.position;
        Physics.IgnoreCollision(cb.GetComponent<Collider>(), Ship);
        cb.GetComponent<Rigidbody>().AddForce(Vertical.forward * 2_000 + Ship.GetComponent<Rigidbody>().velocity);
        //Fire.Play();
    }
}
