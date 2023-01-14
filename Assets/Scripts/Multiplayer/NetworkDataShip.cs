using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataShip : NetworkBehaviour
{
    //SHIP MOVEMENT
    public Rigidbody HostShip;
    [SerializeField] private SailsManager SailsManager;

    private NetworkVariable<Vector2> ShipPosition = new NetworkVariable<Vector2>();
    private NetworkVariable<float> ForwardForce = new NetworkVariable<float>();
    private NetworkVariable<float> ShipRotation = new NetworkVariable<float>();

    public Rigidbody ClientShip;

    //SAIL ROTATION
    public Transform HostBackBoomRing; 
    public Transform HostFrontBoomRing;

    private NetworkVariable<float> BackSailDegrees = new NetworkVariable<float>();
    private NetworkVariable<float> FrontSailDegrees= new NetworkVariable<float>();

    public Transform ClientBackBoomRing;
    public Transform ClientFrontBoomRing;

    //SAIL SCALE
    public Transform HostBackSail;
    public Transform HostFrontSail;
    public Transform HostFrontSailRings;

    private NetworkVariable<float> BackSailScaleX = new NetworkVariable<float>();
    private NetworkVariable<Vector2> FrontSailScaleYZ = new NetworkVariable<Vector2>();
    private NetworkVariable<float> FrontSailRingsPosY = new NetworkVariable<float>();

    public Transform ClientBackSail;
    public Transform ClientFrontSail;
    public Transform ClientFrontSailRings;

    //WIND HELPERS
    public Transform HostWindIndicator;
    public Transform HostWindString;

    private NetworkVariable<float> WindIndicator = new NetworkVariable<float>();
    private NetworkVariable<float> WindString = new NetworkVariable<float>();

    public Transform ClientWindIndicator;
    public Transform ClientWindString;

    //SHIP CONTROLS
    public Transform HostRudder;   
    public Transform HostWheel; 
    public Transform HostSheetRoll;

    private NetworkVariable<float> Rudder = new NetworkVariable<float>();
    private NetworkVariable<float> Wheel = new NetworkVariable<float>();
    private NetworkVariable<float> SheetRoll = new NetworkVariable<float>();

    public Transform ClientRudder;
    public Transform ClientWheel;
    public Transform ClientSheetRoll;

    public override void OnNetworkSpawn()
    {
        ShipRotation.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientShip.transform.eulerAngles = new Vector3(0f, ShipRotation.Value, 0f);
            }
        };

        BackSailDegrees.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientBackBoomRing.localEulerAngles = new Vector3(0f, BackSailDegrees.Value, 0f);
            }
        };

        FrontSailDegrees.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientFrontBoomRing.localEulerAngles = new Vector3(0f, FrontSailDegrees.Value, 0f);
            }
        };

        BackSailScaleX.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientBackSail.localScale = new Vector3(BackSailScaleX.Value, 1f, 1f);
            }
        };

        FrontSailScaleYZ.OnValueChanged += (Vector2 prev, Vector2 current) =>
        {
            if (!IsOwner)
            {
                ClientFrontSail.localScale = new Vector3(1f, FrontSailScaleYZ.Value.x, FrontSailScaleYZ.Value.y);
            }
        };

        FrontSailRingsPosY.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientFrontSailRings.localPosition = new Vector3(0f, FrontSailRingsPosY.Value, 0f);
            }
        };

        WindIndicator.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientWindIndicator.localEulerAngles = new Vector3(0f, WindIndicator.Value, 0f);
            }
        };

        WindString.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientWindString.localEulerAngles = new Vector3(0f, WindString.Value, 0f);
            }
        };

        Rudder.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientRudder.localEulerAngles = new Vector3(0f, Rudder.Value, 0f);
            }
        };

        Wheel.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientWheel.localEulerAngles = new Vector3(0f, 0f, Wheel.Value);
            }
        };

        SheetRoll.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientSheetRoll.localEulerAngles = new Vector3(SheetRoll.Value, 0f, 0f);
            }
        };

        if (!IsOwner)
        {
            InvokeRepeating("SyncShip", Time.time, 5);
        }         
    }

    void Update()
    {
        if (IsOwner)
        {
            ShipPosition.Value = new Vector2(HostShip.position.x, HostShip.position.z);
            ForwardForce.Value = SailsManager.ForwardForce * SailsManager.WindStrength;
            ShipRotation.Value = HostShip.transform.eulerAngles.y;

            BackSailDegrees.Value = HostBackBoomRing.localEulerAngles.y;
            FrontSailDegrees.Value = HostFrontBoomRing.localEulerAngles.y;

            BackSailScaleX.Value = HostBackSail.localScale.x;
            FrontSailScaleYZ.Value = new Vector2(HostFrontSail.localScale.y, HostFrontSail.localScale.z);
            FrontSailRingsPosY.Value = HostFrontSailRings.localPosition.y;

            WindIndicator.Value = HostWindIndicator.localEulerAngles.y;
            WindString.Value = HostWindString.localEulerAngles.y;

            Rudder.Value = HostRudder.localEulerAngles.y;
            Wheel.Value = HostWheel.localEulerAngles.z;
            SheetRoll.Value = HostSheetRoll.localEulerAngles.x;
        }
        else
        {
            ClientShip.GetComponent<Rigidbody>().AddForce(ClientShip.transform.forward * ForwardForce.Value);
        }
    }

    void SyncShip()
    {
        if (!IsOwner)
        {
            ClientShip.position = new Vector3(ShipPosition.Value.x, ClientShip.position.y, ShipPosition.Value.y);
            ClientShip.transform.eulerAngles = new Vector3(0f, ShipRotation.Value, 0f);  
        }
    }
}
