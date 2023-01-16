using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataShip : NetworkBehaviour
{
    //SHIP MOVEMENT
    [SerializeField] private Rigidbody HostShip;
    [SerializeField] private SailsManager SailsManager;

    private NetworkVariable<Vector2> ShipPosition = new NetworkVariable<Vector2>();
    private NetworkVariable<float> ForwardForce = new NetworkVariable<float>();
    private NetworkVariable<float> ShipRotation = new NetworkVariable<float>();

    [SerializeField] private Rigidbody ClientShip;

    //SAIL ROTATION
    [SerializeField] private Transform HostBackBoomRing;
    [SerializeField] private Transform HostFrontBoomRing;

    private NetworkVariable<float> BackSailDegrees = new NetworkVariable<float>();
    private NetworkVariable<float> FrontSailDegrees= new NetworkVariable<float>();

    [SerializeField] private Transform ClientBackBoomRing;
    [SerializeField] private Transform ClientFrontBoomRing;

    //SAIL SCALE
    [SerializeField] private Transform HostBackSail;
    [SerializeField] private Transform HostFrontSail;
    [SerializeField] private Transform HostFrontSailRings;

    private NetworkVariable<float> BackSailScaleX = new NetworkVariable<float>();
    private NetworkVariable<Vector2> FrontSailScaleYZ = new NetworkVariable<Vector2>();
    private NetworkVariable<float> FrontSailRingsPosY = new NetworkVariable<float>();

    [SerializeField] private Transform ClientBackSail;
    [SerializeField] private Transform ClientFrontSail;
    [SerializeField] private Transform ClientFrontSailRings;

    //WIND HELPERS
    [SerializeField] private Transform HostWindIndicator;
    [SerializeField] private Transform HostWindString;

    private NetworkVariable<float> WindIndicator = new NetworkVariable<float>();
    private NetworkVariable<float> WindString = new NetworkVariable<float>();

    [SerializeField] private Transform ClientWindIndicator;
    [SerializeField] private Transform ClientWindString;

    //SHIP CONTROLS
    [SerializeField] private Transform HostRudder;
    [SerializeField] private Transform HostWheel;
    [SerializeField] private Transform HostSheetRoll;

    private NetworkVariable<float> Rudder = new NetworkVariable<float>();
    private NetworkVariable<float> Wheel = new NetworkVariable<float>();
    private NetworkVariable<float> SheetRoll = new NetworkVariable<float>();

    [SerializeField] private Transform ClientRudder;
    [SerializeField] private Transform ClientWheel;
    [SerializeField] private Transform ClientSheetRoll;

    //SHEET
    private NetworkVariable<float> SheetLength = new NetworkVariable<float>();
    [SerializeField] private List<MovingRope> ClientSheetRopes;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            ShipRotation.OnValueChanged += (float prev, float current) =>
            {
                ClientShip.transform.eulerAngles = new Vector3(0f, ShipRotation.Value, 0f);
            };

            BackSailDegrees.OnValueChanged += (float prev, float current) =>
            {
                ClientBackBoomRing.localEulerAngles = new Vector3(0f, BackSailDegrees.Value, 0f);
            };

            FrontSailDegrees.OnValueChanged += (float prev, float current) =>
            {
                ClientFrontBoomRing.localEulerAngles = new Vector3(0f, FrontSailDegrees.Value, 0f);
            };

            BackSailScaleX.OnValueChanged += (float prev, float current) =>
            {
                ClientBackSail.localScale = new Vector3(BackSailScaleX.Value, 1f, 1f);
            };

            FrontSailScaleYZ.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientFrontSail.localScale = new Vector3(1f, FrontSailScaleYZ.Value.x, FrontSailScaleYZ.Value.y);
            };

            FrontSailRingsPosY.OnValueChanged += (float prev, float current) =>
            {
                ClientFrontSailRings.localPosition = new Vector3(0f, FrontSailRingsPosY.Value, 0f);
            };

            WindIndicator.OnValueChanged += (float prev, float current) =>
            {
                ClientWindIndicator.localEulerAngles = new Vector3(0f, WindIndicator.Value, 0f);
            };

            WindString.OnValueChanged += (float prev, float current) =>
            {
                ClientWindString.localEulerAngles = new Vector3(0f, WindString.Value, 0f);
            };

            Rudder.OnValueChanged += (float prev, float current) =>
            {
                ClientRudder.localEulerAngles = new Vector3(0f, Rudder.Value, 0f);
            };

            Wheel.OnValueChanged += (float prev, float current) =>
            {
                ClientWheel.localEulerAngles = new Vector3(0f, 0f, Wheel.Value);
            };

            SheetRoll.OnValueChanged += (float prev, float current) =>
            {
                ClientSheetRoll.localEulerAngles = new Vector3(SheetRoll.Value, 0f, 0f);
            };

            SheetLength.OnValueChanged += (float prev, float current) =>
            {          
                foreach (MovingRope mr in ClientSheetRopes)
                {
                    mr.ClientSheetLength = SheetLength.Value;
                }
            };

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

            SheetLength.Value = SailsManager.SheetLength;    
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            ClientShip.GetComponent<Rigidbody>().AddForce(ClientShip.transform.forward * ForwardForce.Value);
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
