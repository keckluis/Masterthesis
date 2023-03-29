using System.Collections;
using UnityEngine;

public class ClientCannon : MonoBehaviour
{
    public GameObject Fuse;
    bool coolDown = false;
    public Transform[] FusePath = new Transform[4];
    [SerializeField] private int currentFusePos = 0;
    public ParticleSystem MuzzleFlash;
    public Animator ShakeAnimator;

    public Transform CannonBallsHolder;
    public GameObject CannonBall;
    public Transform Vertical;
    public Rigidbody Ship;

    private void FixedUpdate()
    {
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
        if (Fuse.transform.parent.GetComponent<AudioSource>())
            Fuse.transform.parent.GetComponent<AudioSource>().enabled = true;
        yield return new WaitForSeconds(3);
        Fuse.SetActive(false);
        MuzzleFlash.Play();
        ShakeAnimator.SetTrigger("ShakeSmall");

        GameObject cb = Instantiate(CannonBall, CannonBallsHolder);
        cb.transform.position = Vertical.position;

        cb.GetComponent<Rigidbody>().velocity = (Vertical.forward * 80f) + Ship.velocity;

        if (GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        if (Fuse.transform.parent.GetComponent<AudioSource>())
            Fuse.transform.parent.GetComponent<AudioSource>().enabled = false;

        yield return new WaitForSeconds(2);
        coolDown = false;
    }
}
