using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterCollider : MonoBehaviour
{
    public GameObject SplashParticle;
    public Transform CannonSplashHolder;

    private Vector3 HitPosition = Vector3.zero;

    public Transform Map;

    private void Update()
    {
        transform.position = Map.position; 
        transform.rotation = Map.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            HitPosition = collision.transform.position;
            GameObject splash = Instantiate(SplashParticle, CannonSplashHolder);
            splash.transform.position = new Vector3(HitPosition.x, 1, HitPosition.z);
            splash.GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
            StartCoroutine(DestroySplash(splash));
        }
    }

    IEnumerator DestroySplash(GameObject splash)
    {
        yield return new WaitForSeconds(1);
        Destroy(splash);
    }
}
