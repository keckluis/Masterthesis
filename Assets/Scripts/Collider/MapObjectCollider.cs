using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectCollider : MonoBehaviour
{
    [SerializeField] private GameObject Particle;
    [SerializeField] private List<Transform> CrashingShips = new List<Transform>();

    private void FixedUpdate()
    {
        foreach (Transform cs in CrashingShips)
        {
            cs.Rotate(cs.up, 1f);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "CannonBall" || collision.gameObject.tag == "CannonBallEnemy")
        {
            Vector3 particlePos = collision.transform.position;
            Vector3 particleDir = collision.gameObject.GetComponent<Rigidbody>().velocity;
            particleDir = new Vector3(-particleDir.x, -particleDir.y, -particleDir.z);

            Destroy(collision.gameObject);

            GameObject particle = Instantiate(Particle);
            particle.transform.position = particlePos;
            particle.transform.rotation = Quaternion.Euler(particleDir);

            particle.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticle(particle));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ship")
        {
            CrashingShips.Add(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            CrashingShips.Remove(collision.transform);
        }
    }

    IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(1);
        Destroy(particle);
    }
}
