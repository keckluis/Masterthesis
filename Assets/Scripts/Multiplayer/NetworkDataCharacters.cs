using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum Character
{
    Captain,
    Sailor1,
    Sailor2
}

public class NetworkDataCharacters : NetworkBehaviour
{
    public Character Character;
    [SerializeField] private Transform XRCaptainHead;
    [SerializeField] private Transform XRSailor1Head;
    [SerializeField] private Transform XRSailor2Head;

    private NetworkVariable<Vector3> CaptainPos = new NetworkVariable<Vector3>();
    private NetworkVariable<Vector3> Sailor1Pos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> Sailor2Pos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);

    private NetworkVariable<Vector3> CaptainRot = new NetworkVariable<Vector3>();
    private NetworkVariable<Vector3> Sailor1Rot = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> Sailor2Rot = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);

    

    [SerializeField] private Transform HostSailor1Head;
    [SerializeField] private Transform HostSailor2Head;

    [SerializeField] private Transform ClientCaptainHead;
    [SerializeField] private Transform ClientSailor1Head;
    [SerializeField] private Transform ClientSailor2Head;

    public override void OnNetworkSpawn()
    {
        if (Character == Character.Captain)
        {
            Sailor1Pos.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                HostSailor1Head.position = Sailor1Pos.Value;
            };
            Sailor1Rot.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                HostSailor1Head.eulerAngles = Sailor1Rot.Value;
            };
            Sailor2Pos.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                HostSailor2Head.position = Sailor2Pos.Value;
            };
            Sailor2Rot.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                HostSailor2Head.eulerAngles = Sailor2Rot.Value;
            };
        }
        else
        {
            CaptainPos.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                ClientCaptainHead.position = CaptainPos.Value;
            };
            CaptainRot.OnValueChanged += (Vector3 prev, Vector3 current) =>
            {
                ClientCaptainHead.eulerAngles = CaptainRot.Value;
            };

            if (Character == Character.Sailor1)
            {
                Sailor2Pos.OnValueChanged += (Vector3 prev, Vector3 current) =>
                {
                    ClientSailor2Head.position = Sailor2Pos.Value;
                };
                Sailor2Rot.OnValueChanged += (Vector3 prev, Vector3 current) =>
                {
                    ClientSailor2Head.eulerAngles = Sailor2Rot.Value;
                };
            }
            else if (Character == Character.Sailor2)
            {
                Sailor1Pos.OnValueChanged += (Vector3 prev, Vector3 current) =>
                {
                    ClientSailor1Head.position = Sailor1Pos.Value;
                };
                Sailor1Rot.OnValueChanged += (Vector3 prev, Vector3 current) =>
                {
                    ClientSailor1Head.eulerAngles = Sailor1Rot.Value;
                };
            }
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            if (Character == Character.Captain)
            {       
                CaptainPos.Value = XRCaptainHead.position;
                CaptainRot.Value = XRCaptainHead.eulerAngles;
            }
            else if (Character == Character.Sailor1)
            {            
                Sailor1Pos.Value = XRSailor1Head.position;
                Sailor1Rot.Value = XRSailor1Head.eulerAngles;
            }
            else if (Character == Character.Sailor2)
            {     
                Sailor2Pos.Value = XRSailor2Head.position; 
                Sailor2Rot.Value = XRSailor2Head.eulerAngles;
            }
        }
    }
}
