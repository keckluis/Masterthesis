using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkDataAudio : NetworkBehaviour
{
    [SerializeField] private ReadMicrocontrollers ReadMicrocontrollers;

    private NetworkVariable<bool> WheelSound = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> SheetRollSound = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> HalyardSound = new NetworkVariable<bool>(false);

    [SerializeField] private AudioSource ClientWheelAudio;
    [SerializeField] private AudioSource ClientSheetRollAudio;
    [SerializeField] private AudioSource ClientHalyardAudio;

    public override void OnNetworkSpawn()
    {
        WheelSound.OnValueChanged += (bool prev, bool current) =>
        {
            if (!IsOwner)
            {
                ClientWheelAudio.enabled = WheelSound.Value;
            }
        };

        SheetRollSound.OnValueChanged += (bool prev, bool current) =>
        {
            if (!IsOwner)
            {
                ClientSheetRollAudio.enabled = SheetRollSound.Value;
            }
        };

        HalyardSound.OnValueChanged += (bool prev, bool current) =>
        {
            if (!IsOwner)
            {
                ClientHalyardAudio.enabled = HalyardSound.Value;
            }
        };
    }

    private void Update()
    {
        if (IsOwner)
        {
            WheelSound.Value = ReadMicrocontrollers.WheelSound;
            SheetRollSound.Value = ReadMicrocontrollers.SheetRollSound;
            HalyardSound.Value = ReadMicrocontrollers.HalyardSound;
        }
    }
}
