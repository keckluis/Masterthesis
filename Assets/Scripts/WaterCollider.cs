using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterCollider : MonoBehaviour
{
    public Transform SplashParticlePort;
    public Transform SplashParticleStarboard;
    private Vector3 HitPositionPort = Vector3.zero;
    private Vector3 HitPositionStarboard = Vector3.zero;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBallPort")
        {
            HitPositionPort = collision.transform.position;
            SplashParticlePort.position = new Vector3(HitPositionPort.x, 1, HitPositionPort.z);
            SplashParticlePort.GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "CannonBallStarboard")
        {
            HitPositionStarboard = collision.transform.position;
            SplashParticleStarboard.position = new Vector3(HitPositionStarboard.x, 1, HitPositionStarboard.z);
            SplashParticleStarboard.GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }
    }
}
