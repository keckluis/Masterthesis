using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkData : NetworkBehaviour
{

    public Transform HostShip;
    private NetworkVariable<Vector2> ShipPosition = new NetworkVariable<Vector2>(Vector2.zero);
    public Transform HostBackBoomRing;
    private NetworkVariable<float> BackSailDegrees = new NetworkVariable<float>(0f);


    public Transform ClientShip;
    public Transform ClientBackBoomRing;

    public override void OnNetworkSpawn()
    {
        ShipPosition.OnValueChanged += (Vector2 prev, Vector2 current) =>
        {
            if (!IsOwner)
            {
                ClientShip.position = new Vector3(ShipPosition.Value.x, ClientShip.position.y, ShipPosition.Value.y);
                ClientBackBoomRing.localEulerAngles = new Vector3(0f, BackSailDegrees.Value, 0f);
            }
        };
    }

    void Update()
    {
        if (IsOwner)
        {
            ShipPosition.Value = new Vector2(HostShip.position.x, HostShip.position.z);
            BackSailDegrees.Value = HostBackBoomRing.localEulerAngles.y;
        }
    }
}
