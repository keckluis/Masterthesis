using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterCollider : MonoBehaviour
{
    public GameObject SplashParticle;
    private Vector3 HitPositionPort = Vector3.zero;
    private Vector3 HitPositionStarboard = Vector3.zero;

    public Transform Map;

    private void Update()
    {
        transform.position = Map.position; 
        transform.rotation = Map.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBallPort")
        {
            HitPositionPort = collision.transform.position;
            GameObject splash = Instantiate(SplashParticle);
            splash.transform.position = new Vector3(HitPositionPort.x, 1, HitPositionPort.z);
            splash.GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "CannonBallStarboard")
        {
            HitPositionStarboard = collision.transform.position;
            GameObject splash = Instantiate(SplashParticle);
            splash.transform.position = new Vector3(HitPositionPort.x, 1, HitPositionPort.z);
            splash.GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }
    }
}
