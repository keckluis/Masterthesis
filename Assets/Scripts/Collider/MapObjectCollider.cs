using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapObjectCollider : MonoBehaviour
{
    public GameObject Particle;
    private Transform CannonBallsHolder;

    private void Start()
    {
        CannonBallsHolder = GameObject.Find("CannonBallsHolder").transform;
    }

    private void Update()
    {
        if (CannonBallsHolder.childCount > 0)
        {
            List<GameObject> cannonBalls = new List<GameObject>();

            for (int i = 0; i < CannonBallsHolder.childCount; i++)
            {
                //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), CannonBallsHolder.GetChild(i).GetComponent<Collider>());
            }
            
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            Debug.Log("HIT: " + collision.gameObject.name);
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

    /*private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "CannonBall")
        {
            Debug.Log("HIT: " + gameObject.name);
            Vector3 particlePos = collider.transform.position;
            Vector3 particleDir = collider.gameObject.GetComponent<Rigidbody>().velocity;
            particleDir = new Vector3(-particleDir.x, -particleDir.y, -particleDir.z);

            Destroy(collider.gameObject);

            GameObject particle = Instantiate(Particle);
            particle.transform.position = particlePos;
            particle.transform.rotation = Quaternion.Euler(particleDir);

            particle.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticle(particle));
        }
    }*/

    IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(1);
        Destroy(particle);
    }
}
