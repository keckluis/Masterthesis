using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySails : MonoBehaviour
{
    [SerializeField] private SailsManager SailsManager;
    [SerializeField] private NetworkDataEnemies NetworkDataEnemies;
    [SerializeField] private Transform FrontSailRing, BackSailRing;
    [SerializeField] private Transform BackSailPort, BackSailStarboard;
    [SerializeField] private Transform FrontSail, BackSail, Jib;
    [SerializeField] private Transform FrontSailRopeRings, BackSailRopeRings;
    private float FrontRingsY, BackRingsY;

    private float ShipDegrees;
    private Vector2 ShipVector;
    private float WindShipAngle;
    private Vector3 JibStartRotation;

    private void Start()
    {
        JibStartRotation = Jib.localEulerAngles;
        FrontRingsY = FrontSailRopeRings.localPosition.y;
        BackRingsY = BackSailRopeRings.localPosition.y;
    }

    [System.Obsolete]
    void Update()
    {
        Vector2 windVector;
        float windStrength;

        if (SailsManager != null)
        {
            windVector = SailsManager.WindVector;
            windStrength = SailsManager.WindStrength;
        }
        else
        {
            windVector = NetworkDataEnemies.ClientWindVector;
            windStrength = NetworkDataEnemies.ClientWindStrength;
        } 

        ShipDegrees = transform.eulerAngles.y;

        float shipRadians = ShipDegrees * (Mathf.PI / 180);
        ShipVector = new Vector2(Mathf.Sin(shipRadians), Mathf.Cos(shipRadians));
        WindShipAngle = Vector2.Angle(windVector, ShipVector);

        WindShipAngle = Vector2.Angle(windVector, ShipVector);

        Vector3 windForce = new Vector3(windVector.x * windStrength, 0, windVector.y * windStrength);

      
        BackSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, BackSailPort.position);
        BackSailRing.GetComponent<Rigidbody>().AddForceAtPosition(windForce, BackSailStarboard.position);

        float backSailRot = BackSailRing.localEulerAngles.y;
        if (backSailRot > 180f)
            backSailRot -= 360f;

        FrontSailRing.localEulerAngles = new Vector3(0, backSailRot, 0);

        Jib.localEulerAngles = JibStartRotation;
        float jibRot = -backSailRot;
        if (jibRot < 0)
            jibRot = -40f - jibRot;
        else
            jibRot = 40f - jibRot;

        Jib.Rotate(0, 0, jibRot, Space.Self);
        float jibScale;
        if (backSailRot < 0)
            jibScale = -1f;
        else
            jibScale = 1f;

        Jib.GetChild(0).localScale = new Vector3(jibScale, 1f, 1f);
        Jib.GetChild(0).GetChild(0).localScale = new Vector3(1f / jibScale, 1f, 1f);

        HingeJoint hinge = BackSailRing.GetComponent<HingeJoint>();
        JointLimits limits = hinge.limits;
        float frontSheet = (WindShipAngle / 180f) * 40f;
        limits.min = -frontSheet;
        limits.max = frontSheet;
        hinge.limits = limits;
    }

    private void FixedUpdate()
    {
        if (WindShipAngle > 135)
        {
            if (FrontSail.localScale.y > 0.1f)
            {
                FrontSail.localScale = new Vector3(1f, FrontSail.localScale.y - 0.005f, 1f);
                FrontSailRopeRings.localPosition = new Vector3(0f, FrontSail.localScale.y * FrontRingsY, 0f);

                BackSail.localScale = new Vector3(1f, BackSail.localScale.y - 0.005f, 1f);
                BackSailRopeRings.localPosition = new Vector3(0f, BackSail.localScale.y * BackRingsY, 0f);
            }
        }
        else
        {
            if (FrontSail.localScale.y < 1f)
            {
                FrontSail.localScale = new Vector3(1f, FrontSail.localScale.y + 0.005f, 1f);
                FrontSailRopeRings.localPosition = new Vector3(0f, FrontSail.localScale.y * FrontRingsY, 0f);

                BackSail.localScale = new Vector3(1f, BackSail.localScale.y + 0.005f, 1f);
                BackSailRopeRings.localPosition = new Vector3(0f, BackSail.localScale.y * BackRingsY, 0f);
            }
        }
    }
}
