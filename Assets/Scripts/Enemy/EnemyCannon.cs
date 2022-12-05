using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyCannon : MonoBehaviour
{
    public Transform Horizontal;
    public Transform Vertical;
    public Transform PlayerShip;

    public float SightDistance = 150f;

    public Transform CanonBallsHolder;
    public GameObject CanonBall;
    public Rigidbody Map;

    public ParticleSystem MuzzleFlash;


    public bool isPort;
    public Vector2 PlayerDirection;
    bool coolDown = false;
    public bool playerInRange = false;
    public bool fightMode = false;

    public float MaxError = 3f;
    float hError = 0f;
    float vError = 0f;

    public EnemyCannon OtherCannon;
    public Transform EnemyShip;

    void FixedUpdate()
    {
        PlayerDirection = new Vector2(PlayerShip.position.x, PlayerShip.position.z) 
                          - new Vector2(transform.position.x, transform.position.z);

        if (PlayerDirection.magnitude < SightDistance)
        {
            fightMode = true;
            float h = Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), PlayerDirection) - 90;

            if (PlayerDirection.magnitude > OtherCannon.PlayerDirection.magnitude)
            {
                playerInRange = false;
            }
            else
            {
                if (h > 45f)
                {
                    playerInRange = false;
                    AdjustCourse(false);
                }
                else if (h < -45f)
                {
                    playerInRange = false;
                    AdjustCourse(true);
                }
                else
                    playerInRange = true;
            }
               
            h += hError;
            h = Mathf.Clamp(h, -45f, 45f);

            if (isPort)
                h = -h;

            Horizontal.localEulerAngles = new Vector3(0f, h, 0f);

            float v = Vector3.Magnitude(PlayerDirection);

            if (v > 30f)
                v = (v / SightDistance) * 30f;
            else
                v = 0f;

            v += vError;
            v = Mathf.Clamp(v, -30f, 30f);

            if (isPort)
                v = -v;

            Vertical.localEulerAngles = new Vector3(0f, 0f, v);

            if (!coolDown && playerInRange)
            {
                Shoot();
            }      
        }
        else
        {
            fightMode = false;
        }
    }

    void AdjustCourse(bool towardsPort)
    {
        if (towardsPort) 
            EnemyShip.localEulerAngles = new Vector3(0f, EnemyShip.localEulerAngles.y + 0.1f, 0f);
        else
            EnemyShip.localEulerAngles = new Vector3(0f, EnemyShip.localEulerAngles.y - 0.1f, 0f);
    }

    public void OnDrawGizmosSelected()
    {
        if (isPort)
            Handles.color = Color.red;
        else
            Handles.color = Color.green;

        Handles.DrawWireDisc(transform.position, Vector3.up,SightDistance);
        //Gizmos.DrawLine(transform.position, PlayerShip.position);
    }

    void Shoot()
    {
        if (!coolDown)
        {
            coolDown = true;
            StartCoroutine(WaitForShot());
        }
    }

    IEnumerator WaitForShot()
    {
        yield return new WaitForSeconds(3);

        MuzzleFlash.Play();

        GameObject cb = Instantiate(CanonBall, CanonBallsHolder);
        cb.transform.position = Vertical.position;

        if (isPort)
            cb.GetComponent<Rigidbody>().AddForce(-Vertical.right * 2_000 + Map.velocity);
        else
            cb.GetComponent<Rigidbody>().AddForce(Vertical.right * 2_000 + Map.velocity);

        yield return new WaitForSeconds(2);

        hError = Random.Range(-MaxError, MaxError);
        vError = Random.Range(-MaxError, MaxError);
        coolDown = false;
    }
}
