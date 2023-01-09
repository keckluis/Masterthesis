using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataHost : NetworkBehaviour
{

    public Transform Ship;
    private NetworkVariable<Vector2> ShipPosition = new NetworkVariable<Vector2>(Vector2.zero);
    public Transform BackBoomRing;
    private NetworkVariable<float> BackSailDegrees = new NetworkVariable<float>(0f);

    public override void OnNetworkSpawn()
    {
        ShipPosition.OnValueChanged += (Vector2 prev, Vector2 current) =>
        {
            //MOVE CLIENT SHIP
        };
    }

    void Update()
    {
        ShipPosition.Value = new Vector2(Ship.position.x, Ship.position.z);
        BackSailDegrees.Value = BackBoomRing.localEulerAngles.y;
    }
}
