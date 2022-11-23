using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Cannon : MonoBehaviour
{
    public Transform Tracker;
    public Transform Horizontal;
    public Transform Vertical;
    public Transform CanonBallsHolder;
    public GameObject CanonBall;
    public Rigidbody Map;

    public ParticleSystem MuzzleFlash;
    public GameObject Fuse;
    public Transform[] FusePath = new Transform[4];
    [SerializeField]private int currentFusePos = 0;

    bool coolDown = false;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            RotateHorizontal(-1f);
        if (Input.GetKey(KeyCode.RightArrow))
            RotateHorizontal(1f);
        if (Input.GetKey(KeyCode.UpArrow))
            RotateVertical(-1f);
        if (Input.GetKey(KeyCode.DownArrow))
            RotateVertical(1f);

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if  (Tracker != null)
        {
            float h = Tracker.localEulerAngles.y;
            float v = Tracker.localEulerAngles.x;

            Horizontal.localEulerAngles = new Vector3(0f, h, 0f);
            Vertical.localEulerAngles = new Vector3(v, 0f, 0f);
        }

        if (Fuse.activeSelf)
        {
            if (currentFusePos < FusePath.Length && Fuse.transform.position != FusePath[currentFusePos].position)
            {
                Vector3 pos = Vector3.MoveTowards(Fuse.transform.position, FusePath[currentFusePos].position, 0.0005f);
                Fuse.transform.position = pos;
            } 
            else
            {
                currentFusePos++;
            }
        }
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
        if (!coolDown)
        {
            coolDown = true;
            StartCoroutine(WaitForShot());
        }
    }

    IEnumerator WaitForShot()
    {
        Fuse.SetActive(true);
        Fuse.transform.position = FusePath[0].position;

        currentFusePos = 0;
        Fuse.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3);
        Fuse.SetActive(false);
        MuzzleFlash.Play();

        GameObject cb = Instantiate(CanonBall, CanonBallsHolder);
        cb.transform.position = Vertical.position;

        //Physics.IgnoreCollision(cb.GetComponent<Collider>(), Map.GetComponent<Collider>());
        cb.GetComponent<Rigidbody>().AddForce(Vertical.forward * 2_000 + Map.velocity);

        yield return new WaitForSeconds(2);
        coolDown = false;
    }
}
