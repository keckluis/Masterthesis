using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectCollider : MonoBehaviour
{
    public GameObject Particle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
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

    IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(1);
        Destroy(particle);
    }
}
