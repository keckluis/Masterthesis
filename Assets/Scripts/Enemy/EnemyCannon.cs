using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] private Transform Horizontal;
    [SerializeField] private Transform Vertical;
    [SerializeField] private Transform PlayerShip;

    private float SightDistance = 135f;

    [SerializeField] private Transform CanonBallsHolder;
    [SerializeField] private GameObject CanonBall;
    [SerializeField] private NavMeshAgent EnemyShipAgent;

    [SerializeField] private ParticleSystem MuzzleFlash;


    [SerializeField] private bool isPort;
    [SerializeField] private Vector2 PlayerDirection;
    private bool coolDown = false;
    [SerializeField] private bool playerInRange = false;
    public bool FightMode = false;

    [SerializeField] private float MaxError = 3f;
    float hError = 0f;
    float vError = 0f;

    [SerializeField] private EnemyCannon OtherCannon;
    [SerializeField] private Animator ShakeAnimator;

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

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (isPort)
            Handles.color = Color.red;
        else
            Handles.color = Color.green;

        Handles.DrawWireDisc(transform.position, Vector3.up,SightDistance);
    }
#endif

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
