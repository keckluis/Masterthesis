using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    public GameObject Particle;
    public GameObject Fire;
    public GameObject CannonPort;
    public GameObject CannonStarboard;
    public bool PlayerShip = false;
    private bool Destroyed = false;
    private Transform Ship;
    public int Health = 2;

    private void FixedUpdate()
    {
        if (Destroyed && Ship != null)
        {
            if (Ship.localEulerAngles.z < 70f)
            {
                Ship.localEulerAngles = new Vector3(Ship.localEulerAngles.x, Ship.localEulerAngles.y, Ship.localEulerAngles.z + 1f);
                Ship.localPosition = new Vector3(Ship.localPosition.x, Ship.localPosition.y - 0.2f, Ship.localPosition.z);
            }
            else
            {
                Destroy(Ship.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            Health -= 1;
            GameObject ship = transform.parent.gameObject;
            print("HIT: " + ship.name);
            Vector3 particlePos = collision.transform.position;
            Vector3 particleDir = collision.gameObject.GetComponent<Rigidbody>().velocity;
            particleDir = new Vector3(-particleDir.x, -particleDir.y, -particleDir.z);

            Destroy(collision.gameObject);

            GameObject particle = Instantiate(Particle);
            particle.transform.position = particlePos;
            particle.transform.rotation = Quaternion.Euler(particleDir);

            particle.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticle(particle));

            if (!PlayerShip)
            {
                if (Health <= 0)
                {
                    Fire.SetActive(false);
                    if (ship.GetComponent<EnemySails>().Path != null)
                        Destroy(ship.GetComponent<EnemySails>().Path.gameObject);
                    Destroy(ship.GetComponent<EnemySails>());
                    Destroy(ship.GetComponent<Floating>());
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
        }

        IEnumerator DestroyParticle(GameObject particle)
        {
            yield return new WaitForSeconds(1);
            Destroy(particle);
        }
    }
}
