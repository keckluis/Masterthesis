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

    public EnemyCannon CannonPort;
    public EnemyCannon CannonStarboard;
    public Transform Forward;
    bool followPath = true;

    public void OnDrawGizmosSelected()
    {
        for (int i = 0; i < PathTargets.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(PathTargets[i].position, 3);
            Gizmos.DrawLine(PathTargets[i].position, PathTargets[(i + 1) % PathTargets.Length].position);
        }
    }

    private void Start()
    {
        FindClosestTarget();
        TargetRotation = CalculateTargetRotation();
    }

    void FixedUpdate()
    {
        if (CannonPort.fightMode || CannonStarboard.fightMode)
        {
            followPath = false;
        }
        if (!CannonPort.fightMode && !CannonStarboard.fightMode && !followPath)
        {
            FindClosestTarget();
            followPath = true;
        }

        if (followPath) { 
            if (Vector3.Distance(EnemyShip.position, PathTargets[Current].position) > 1)
            {
                Vector3 pos = Vector3.MoveTowards(EnemyShip.position, PathTargets[Current].position, SailsManager.WindStrength * 0.05f);
                EnemyShip.position = pos;
                TargetRotation = CalculateTargetRotation();
                EnemyShip.rotation = Quaternion.RotateTowards(EnemyShip.rotation, Quaternion.Euler(TargetRotation), SailsManager.WindStrength * 0.05f);
            }
            else
            {
                Current = (Current + 1) % PathTargets.Length;
            }

            if (EnemyShip.eulerAngles.y > TargetRotation.y + 1f)
            {
                Rudder.localEulerAngles = new Vector3(0f, 20f, 0f);
            }
            else if (EnemyShip.eulerAngles.y < TargetRotation.y - 1f)
            {
                Rudder.localEulerAngles = new Vector3(0f, -20f, 0f);
            }
            else
            {
                Rudder.localEulerAngles = Vector3.zero;
            }
        }
        else
        {
            Vector3 pos = Vector3.MoveTowards(EnemyShip.position, Forward.position, SailsManager.WindStrength * 0.05f);
            EnemyShip.position = pos;
        }
    }

    void FindClosestTarget()
    {
        float minDistance = Vector3.Distance(PathTargets[0].position, EnemyShip.position);
        for (int i = 1; i < PathTargets.Length; i++)
        {
            float distance = Vector3.Distance(PathTargets[i].position, EnemyShip.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                Current = i;
            }
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
