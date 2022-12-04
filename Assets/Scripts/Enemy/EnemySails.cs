using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySails : MonoBehaviour
{
    public SailsManager SailsManager;
    public Transform FrontSailRing;
    public Transform BackSailRing;
    public Transform BackSailPort;
    public Transform BackSailStarboard;

    public EnemyFollowPath Path;

    float ShipDegrees;
    Vector2 ShipVector;
    float WindShipAngle;
    //bool WindFromFront;

    void Update()
    {
        Vector2 windVector = SailsManager.WindVector;
        float windStrength = SailsManager.WindStrength;

        ShipDegrees = transform.eulerAngles.y;

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        ShipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));
        WindShipAngle = Vector2.Angle(windVector, ShipVector);

        WindShipAngle = Vector2.Angle(windVector, ShipVector);

        Vector3 windForce = new Vector3(windVector.x * windStrength, 0, windVector.y * windStrength);

        if (WindShipAngle > 135)
        {
           // WindFromFront = true;
        }
        else
        {
            //WindFromFront = false;
        }

        //Vector3 speed = GetComponent<Rigidbody>().velocity;
        //Vector3 speedAngular = GetComponent<Rigidbody>().angularVelocity;

        BackSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, BackSailPort.position);
        BackSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, BackSailStarboard.position);
        //GetComponent<Rigidbody>().AddForce(windForce * -2f);

        FrontSailRing.localEulerAngles = new Vector3(0, BackSailRing.localEulerAngles.y, 0);

        HingeJoint hinge = BackSailRing.GetComponent<HingeJoint>();
        JointLimits limits = hinge.limits;
        float frontSheet = (WindShipAngle / 180f) * 40f;
        limits.min = -frontSheet;
        limits.max = frontSheet;
        hinge.limits = limits;

        //GetComponent<Rigidbody>().velocity = speed;
        //GetComponent<Rigidbody>().angularVelocity = speedAngular;
    }

    public void OnDrawGizmosSelected()
    {
        if (Path != null)
        {
            for (int i = 0; i < Path.PathTargets.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(Path.PathTargets[i].position, 3);
                Gizmos.DrawLine(Path.PathTargets[i].position, Path.PathTargets[(i + 1) % Path.PathTargets.Length].position);
            }
        }    
    }
}
