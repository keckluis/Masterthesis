using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform TargetParent;
    public List<Transform> Targets;
    public int Current;

    public bool FightMode = false;
    private bool FollowPath = true;

    public SailsManager SailsManager;
    public Transform BackSail;
    public Transform Forward;
    public EnemyCannon CannonPort, CannonStarboard;

    public NavMeshAgent Agent;

    void Start()
    {
        if (TargetParent != null)
        {
            Targets = new List<Transform>();

            for (int i = 0; i < TargetParent.childCount; i++)
            {
                Targets.Add(TargetParent.GetChild(i));
            }
        }

        float distance = 0f;
        int j = 0;
        foreach(Transform t in Targets)
        {
            if (Vector3.Distance(transform.position, t.position) < distance || j == 0)
            {
                distance = Vector3.Distance(transform.position, t.position);
                Current = j;
                j++;
            }
        }

        Agent.destination = Targets[Current].position;
    }

    void FixedUpdate()
    {
        if (CannonPort.FightMode || CannonStarboard.FightMode)
            FightMode = true;
        else
            FightMode = false;

        if (!FightMode)
        {
            FollowPath = true;
            if (Agent.remainingDistance < 1f)
            {
                Current = (Current + 1) % Targets.Count;
                Agent.destination = Targets[Current].position;
            }
        }
        else
        {
            if (FollowPath)
            {
                FollowPath = false;
                Agent.destination = Forward.position;
            }
            else
            {
                if (Agent.remainingDistance < 1f)
                {
                    Agent.destination = Forward.position;
                }
            }
        }

        float sailSize = BackSail.localScale.y;
        sailSize = Mathf.Clamp(sailSize, 0.5f, 1f);
        Agent.speed = (SailsManager.WindStrength * sailSize) * 2.5f;
        Agent.acceleration = SailsManager.WindStrength;
        Agent.angularSpeed = Agent.speed;

        transform.position = Agent.transform.position;
        transform.rotation = Agent.transform.rotation;
    }

    public void OnDrawGizmosSelected()
    {
        if (TargetParent != null)
        {
            for (int i = 0; i < TargetParent.childCount; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(TargetParent.GetChild(i).position, 3);
                Gizmos.DrawLine(TargetParent.GetChild(i).position, TargetParent.GetChild((i + 1) % TargetParent.childCount).position);
            }
        }
        else
        {
            for (int i = 0; i < Targets.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(Targets[i].position, 3);
                Gizmos.DrawLine(Targets[i].position, Targets[(i + 1) % Targets.Count].position);
            }
        }
           
    }
}