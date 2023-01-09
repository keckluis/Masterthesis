using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataClient : NetworkBehaviour
{

    public Transform Player;
    private NetworkVariable<Vector3> ClientPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Transform BackBoomRing;
    private NetworkVariable<Vector3> ClientRotation = new NetworkVariable<Vector3>(Vector2.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Update()
    {
        ClientPosition.Value = Player.position;
        ClientRotation.Value = Player.eulerAngles;
    }
}
