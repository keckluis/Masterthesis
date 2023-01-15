using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    [SerializeField] private GameObject Particle;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "CannonBallEnemy")
        {
            GameObject ship = gameObject;
            print("HIT: " + ship.name);
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shake");
            Vector3 impactPos = collision.transform.position;
            Vector3 impactDir = collision.gameObject.GetComponent<Rigidbody>().velocity;

            Destroy(collision.gameObject);

            GameObject particle = Instantiate(Particle);
            particle.transform.position = impactPos;
            particle.transform.rotation = Quaternion.Euler(-impactDir);

            particle.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticle(particle));

            GetComponent<AudioSource>().Play();
        }

        IEnumerator DestroyParticle(GameObject particle)
        {
            yield return new WaitForSeconds(1);
            Destroy(particle);
        }
    }
}
