using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDataEnemies : NetworkBehaviour
{

    [SerializeField] private List<Transform> HostEnemyShips;

    private NetworkVariable<Vector2> Enemy0Pos = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> Enemy1Pos = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> Enemy2Pos = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> Enemy3Pos = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> Enemy4Pos = new NetworkVariable<Vector2>();

    private NetworkVariable<float> Enemy0Rot = new NetworkVariable<float>();
    private NetworkVariable<float> Enemy1Rot = new NetworkVariable<float>();
    private NetworkVariable<float> Enemy2Rot = new NetworkVariable<float>();
    private NetworkVariable<float> Enemy3Rot = new NetworkVariable<float>();
    private NetworkVariable<float> Enemy4Rot = new NetworkVariable<float>();

    [SerializeField] private List<Transform> ClientEnemyShips;

    [SerializeField] private SailsManager SailsManager;
    private NetworkVariable<Vector2> WindVector = new NetworkVariable<Vector2>();
    private NetworkVariable<float> WindStrength = new NetworkVariable<float>();
    public Vector2 ClientWindVector;
    public float ClientWindStrength;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Enemy0Pos.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientEnemyShips[0].position = new Vector3(Enemy0Pos.Value.x, ClientEnemyShips[0].position.y, Enemy0Pos.Value.y);
            };
            Enemy1Pos.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientEnemyShips[1].position = new Vector3(Enemy1Pos.Value.x, ClientEnemyShips[1].position.y, Enemy1Pos.Value.y);
            };
            Enemy2Pos.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientEnemyShips[2].position = new Vector3(Enemy2Pos.Value.x, ClientEnemyShips[2].position.y, Enemy2Pos.Value.y);
            };
            Enemy3Pos.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientEnemyShips[3].position = new Vector3(Enemy3Pos.Value.x, ClientEnemyShips[3].position.y, Enemy3Pos.Value.y);
            };
            Enemy4Pos.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientEnemyShips[4].position = new Vector3(Enemy4Pos.Value.x, ClientEnemyShips[4].position.y, Enemy4Pos.Value.y);
            };

            Enemy0Rot.OnValueChanged += (float prev, float current) =>
            {
                ClientEnemyShips[0].transform.eulerAngles = new Vector3(0f, Enemy0Rot.Value, 0f);
            };
            Enemy1Rot.OnValueChanged += (float prev, float current) =>
            {
                ClientEnemyShips[1].transform.eulerAngles = new Vector3(0f, Enemy1Rot.Value, 0f);
            };
            Enemy2Rot.OnValueChanged += (float prev, float current) =>
            {
                ClientEnemyShips[2].transform.eulerAngles = new Vector3(0f, Enemy2Rot.Value, 0f);
            };
            Enemy3Rot.OnValueChanged += (float prev, float current) =>
            {
                ClientEnemyShips[3].transform.eulerAngles = new Vector3(0f, Enemy3Rot.Value, 0f);
            };
            Enemy4Rot.OnValueChanged += (float prev, float current) =>
            {
                ClientEnemyShips[4].transform.eulerAngles = new Vector3(0f, Enemy4Rot.Value, 0f);
            };

            WindVector.OnValueChanged += (Vector2 prev, Vector2 current) =>
            {
                ClientWindVector = WindVector.Value;
            };
            WindStrength.OnValueChanged += (float prev, float current) =>
            {
                ClientWindStrength = WindStrength.Value;
            };
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            Enemy0Pos.Value = new Vector2(HostEnemyShips[0].position.x, HostEnemyShips[0].position.z);
            Enemy1Pos.Value = new Vector2(HostEnemyShips[1].position.x, HostEnemyShips[1].position.z);
            Enemy2Pos.Value = new Vector2(HostEnemyShips[2].position.x, HostEnemyShips[2].position.z);
            Enemy3Pos.Value = new Vector2(HostEnemyShips[3].position.x, HostEnemyShips[3].position.z);
            Enemy4Pos.Value = new Vector2(HostEnemyShips[4].position.x, HostEnemyShips[4].position.z);

            Enemy0Rot.Value = HostEnemyShips[0].transform.eulerAngles.y;
            Enemy1Rot.Value = HostEnemyShips[1].transform.eulerAngles.y;
            Enemy2Rot.Value = HostEnemyShips[2].transform.eulerAngles.y;
            Enemy3Rot.Value = HostEnemyShips[3].transform.eulerAngles.y;
            Enemy4Rot.Value = HostEnemyShips[4].transform.eulerAngles.y;

            WindVector.Value = SailsManager.WindVector;
            WindStrength.Value = SailsManager.WindStrength;
        }
    }
}
