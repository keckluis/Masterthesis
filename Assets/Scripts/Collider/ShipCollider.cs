using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    [SerializeField] private GameObject Particle;

    public GameObject[] Plunder = new GameObject[5];
    public int PlunderAmount = 0;

    void OnTriggerEnter(Collider collision)
    {
        print(collision.name);
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
        else if (collision.gameObject.tag == "Plunder")
        {
            Destroy(collision.gameObject);

            if (PlunderAmount < 5)
            {
                Plunder[PlunderAmount].SetActive(true);

                PlunderAmount++;
            }    
        }
    }

    IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(1);
        Destroy(particle);
    }
}
