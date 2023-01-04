using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShipCollider : MonoBehaviour
{
    public GameObject Particle;
    public GameObject Fire;
    public GameObject CannonPort;
    public GameObject CannonStarboard;
    private bool Destroyed = false;
    private Transform Ship;
    public int Health = 2;
    public GameObject BackSailring;

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
                Destroy(Ship.transform.parent.gameObject);
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
