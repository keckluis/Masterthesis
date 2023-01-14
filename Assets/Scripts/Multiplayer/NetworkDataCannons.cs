using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataCannons : NetworkBehaviour
{
    //CANNON ROTATION
    public Transform HostPortHorizontal;
    public Transform HostPortVertical;
    public Transform HostStarboardHorizontal;
    public Transform HostStarboardVertical;

    private NetworkVariable<float> PortHorizontal = new NetworkVariable<float>();
    private NetworkVariable<float> PortVertical = new NetworkVariable<float>();
    private NetworkVariable<float> StarboardHorizontal = new NetworkVariable<float>();
    private NetworkVariable<float> StarboardVertical = new NetworkVariable<float>();

    public Transform ClientPortHorizontal;
    public Transform ClientPortVertical;
    public Transform ClientStarboardHorizontal;
    public Transform ClientStarboardVertical;

    //CANNON SHOOT
    private NetworkVariable<bool> PortCoolDown = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> StarboardCoolDown = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        PortHorizontal.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientPortHorizontal.localEulerAngles = new Vector3(0f, PortHorizontal.Value, 0f);
            }
        };

        PortVertical.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientPortVertical.localEulerAngles = new Vector3(PortVertical.Value, 0f, 0f);
            }
        };

        StarboardHorizontal.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientStarboardHorizontal.localEulerAngles = new Vector3(0f, StarboardHorizontal.Value, 0f);
            }
        };

        StarboardVertical.OnValueChanged += (float prev, float current) =>
        {
            if (!IsOwner)
            {
                ClientStarboardVertical.localEulerAngles = new Vector3(StarboardVertical.Value, 0f, 0f);
            }
        };

        PortCoolDown.OnValueChanged += (bool prev, bool current) =>
        {
            if (!IsOwner)
            {
                if (PortCoolDown.Value)
                {
                    ClientPortHorizontal.parent.GetComponent<ClientCannon>().Shoot();
                }
            }
        };

        StarboardCoolDown.OnValueChanged += (bool prev, bool current) =>
        {
            if (!IsOwner)
            {
                if (PortCoolDown.Value)
                {
                    ClientStarboardHorizontal.parent.GetComponent<ClientCannon>().Shoot();
                }
            }
        };
    }

    void Update()
    {
        if (IsOwner)
        {
            PortHorizontal.Value = HostPortHorizontal.localEulerAngles.y;
            PortVertical.Value = HostPortVertical.localEulerAngles.x;

            StarboardHorizontal.Value = HostStarboardHorizontal.localEulerAngles.y;
            StarboardVertical.Value = HostStarboardVertical.localEulerAngles.x;

            PortCoolDown.Value = HostPortHorizontal.parent.GetComponent<Cannon>().coolDown;
            StarboardCoolDown.Value = HostStarboardHorizontal.parent.GetComponent<Cannon>().coolDown;
        }
    }
}
