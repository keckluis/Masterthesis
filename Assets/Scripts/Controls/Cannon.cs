using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainTools;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform Tracker;
    [SerializeField] private Transform Horizontal;
    [SerializeField] private Transform Vertical;
    [SerializeField] private Transform CannonBallsHolder;
    [SerializeField] private GameObject CannonBall;
    [SerializeField] private Rigidbody Ship;

    [SerializeField] private ParticleSystem MuzzleFlash;
    [SerializeField] private GameObject Fuse;
    [SerializeField] private Transform[] FusePath = new Transform[4];
    [SerializeField] private int currentFusePos = 0;

    public bool coolDown = false;
    [SerializeField] private Animator ShakeAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }
    void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.LeftArrow))
            RotateHorizontal(-1f);
        if (Input.GetKey(KeyCode.RightArrow))
            RotateHorizontal(1f);
        if (Input.GetKey(KeyCode.UpArrow))
            RotateVertical(-1f);
        if (Input.GetKey(KeyCode.DownArrow))
            RotateVertical(1f);*/
        

        if  (Tracker != null)
        {
            float h = Tracker.localEulerAngles.z;
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
        Vertical.Rotate(new Vector3(direction, 0f, 0f));

        if (Vertical.localEulerAngles.x > 25f && Vertical.localEulerAngles.x < 180f)
        {
            Vertical.localEulerAngles = new Vector3(25f, 0f, 0f);
        }
        else if (Vertical.localEulerAngles.x < 335f && Vertical.localEulerAngles.x > 180f)
        {
           Vertical.localEulerAngles = new Vector3(335f, 0f, 0f);
        }
    }   

    public void Shoot()
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
        if(Fuse.transform.parent.GetComponent<AudioSource>())
            Fuse.transform.parent.GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(3);
        Fuse.SetActive(false);
        MuzzleFlash.Play();
        ShakeAnimator.SetTrigger("ShakeSmall");

        GameObject cb = Instantiate(CannonBall, CannonBallsHolder);
        cb.transform.position = Vertical.position;

        cb.GetComponent<Rigidbody>().velocity = (Vertical.forward * 40f) + Ship.velocity;

        if(GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        if (Fuse.transform.parent.GetComponent<AudioSource>())
            Fuse.transform.parent.GetComponent<AudioSource>().enabled = false;

        yield return new WaitForSeconds(2);
        coolDown = false;
    }
}
