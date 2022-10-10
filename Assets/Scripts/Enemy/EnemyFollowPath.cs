using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPath : MonoBehaviour
{
    public Transform EnemyShip;
    public Transform[] PathTargets;
    public SailsManager SailsManager;
    public Transform Rudder;

    private int Current = 0;
    private Vector3 TargetRotation;

    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < PathTargets.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(PathTargets[i].position, 5);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(PathTargets[i].position, PathTargets[(i + 1) % PathTargets.Length].position);          
        }
    }

    private void Start()
    {
        TargetRotation = CalculateTargetRotation();
    }

    void FixedUpdate()
    {
        if (EnemyShip.position.x != PathTargets[Current].position.x && EnemyShip.position.z != PathTargets[Current].position.z)
        {
            Vector3 pos = Vector3.MoveTowards(EnemyShip.position, PathTargets[Current].position, SailsManager.WindStrength * 0.1f);
            EnemyShip.GetComponent<Rigidbody>().MovePosition(pos);
            TargetRotation = CalculateTargetRotation();
            EnemyShip.rotation = Quaternion.RotateTowards(EnemyShip.rotation, Quaternion.Euler(TargetRotation), SailsManager.WindStrength);
        }
        else 
        {
            Current = (Current + 1) % PathTargets.Length;
        }

        if (EnemyShip.eulerAngles != TargetRotation)
        {
            Rudder.localEulerAngles = new Vector3(0, -20, 0);
        }
        else
        {
            Rudder.localEulerAngles = Vector3.zero;
        }
    }

    private Vector3 CalculateTargetRotation()
    {
        Vector3 shipRot = EnemyShip.eulerAngles;
        EnemyShip.LookAt(PathTargets[Current]);
        Vector3 targetRot = EnemyShip.eulerAngles;
        EnemyShip.eulerAngles = shipRot;
        return new Vector3 (0, targetRot.y, 0);
    }
}
