using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class EnemyCannon : MonoBehaviour
{
    public Transform Horizontal;
    public Transform Vertical;
    public Transform PlayerShip;

    private float SightDistance = 135f;

    public Transform CanonBallsHolder;
    public GameObject CanonBall;
    public NavMeshAgent EnemyShipAgent;

    public ParticleSystem MuzzleFlash;


    public bool isPort;
    public Vector2 PlayerDirection;
    bool coolDown = false;
    public bool playerInRange = false;
    public bool FightMode = false;

    public float MaxError = 3f;
    float hError = 0f;
    float vError = 0f;

    public EnemyCannon OtherCannon;
    public Animator ShakeAnimator;

    void FixedUpdate()
    {
        PlayerDirection = new Vector2(PlayerShip.position.x, PlayerShip.position.z) 
                          - new Vector2(transform.position.x, transform.position.z);

        if (PlayerDirection.magnitude < SightDistance)
        {
            FightMode = true;
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

            float v = (Vector3.Magnitude(PlayerDirection) / SightDistance) * 30f;
            v -= 5f;
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
            FightMode = false;
        }
    }

    void AdjustCourse(bool towardsPort)
    {
        if (towardsPort)
            EnemyShipAgent.transform.Rotate(EnemyShipAgent.transform.up, 0.1f);
        else
            EnemyShipAgent.transform.Rotate(EnemyShipAgent.transform.up, -0.1f);    
    }

    public void OnDrawGizmosSelected()
    {
        if (isPort)
            Handles.color = Color.red;
        else
            Handles.color = Color.green;

        Handles.DrawWireDisc(transform.position, Vector3.up,SightDistance);
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
        ShakeAnimator.SetTrigger("ShakeSmall");

        GameObject cb = Instantiate(CanonBall, CanonBallsHolder);
        cb.transform.position = Vertical.position;

        if (isPort)
            cb.GetComponent<Rigidbody>().velocity = (-Vertical.right * 40f) + EnemyShipAgent.velocity;
        else
            cb.GetComponent<Rigidbody>().velocity = (Vertical.right * 40f) + EnemyShipAgent.velocity;

        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(2);

        hError = Random.Range(-MaxError, MaxError);
        vError = Random.Range(-MaxError, MaxError);
        coolDown = false;
    }
}
