using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShipCollider : MonoBehaviour
{
    [SerializeField] private GameObject Particle;
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject CannonPort;
    [SerializeField] private GameObject CannonStarboard;
    private bool Destroyed = false;
    private Transform Ship;
    [SerializeField] private int Health = 2;
    [SerializeField] private GameObject BackSailring;
    [SerializeField] private GameObject Plunder;

    private void FixedUpdate()
    {
        if (Destroyed && Ship != null)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            Destroy(BackSailring.GetComponent<HingeJoint>());
            Destroy(BackSailring.GetComponent<Rigidbody>());
            Destroy(BackSailring.GetComponent<Collider>());
            if (Ship.localEulerAngles.z < 70f)
            {
                Ship.localEulerAngles = new Vector3(Ship.localEulerAngles.x, Ship.localEulerAngles.y, Ship.localEulerAngles.z + 0.25f);
                Ship.localPosition = new Vector3(Ship.localPosition.x, Ship.localPosition.y - 0.05f, Ship.localPosition.z);
            }
            else
            {
                Vector3 shipPos = Ship.position;
                Destroy(Ship.transform.parent.gameObject);
                GameObject plunder = Instantiate(Plunder);
                plunder.transform.position = new Vector3(shipPos.x, 0f, shipPos.z);
                plunder.transform.localEulerAngles = new Vector3(0f, Random.Range(0f, 360f), 0f);
            }
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            Health -= 1;
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

            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().Play();
            
            if (Health <= 0)
            {
                Fire.SetActive(false);
                Destroy(ship.GetComponent<EnemySails>());
                Destroy(ship.GetComponent<Floating>());
                Destroy(ship.GetComponent<EnemyAI>().Agent.gameObject);
                Destroy(ship.GetComponent<EnemyAI>());
                Destroy(CannonPort.GetComponent<EnemyCannon>());
                Destroy(CannonStarboard.GetComponent<EnemyCannon>());
                Ship = ship.transform;
                Destroyed = true;
            }
            else if (Health == 1)
            {
                Fire.SetActive(true);
            }
            
        }

        IEnumerator DestroyParticle(GameObject particle)
        {
            yield return new WaitForSeconds(1);
            Destroy(particle);
        }
    }
}
