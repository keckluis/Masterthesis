using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System;
using TMPro;
using static Unity.Netcode.Transports.UTP.UnityTransport;
using System.Net;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private TextMeshProUGUI IP;
    [SerializeField] private Button Client1Btn;
    [SerializeField] private Button Client2Btn;

    [SerializeField] private GameObject Host;
    [SerializeField] private GameObject Client;

    [SerializeField] private GameObject Sailor1;
    [SerializeField] private GameObject Sailor2;
    [SerializeField] private GameObject Sailor1Head;
    [SerializeField] private GameObject Sailor2Head;


    [SerializeField] private NetworkDataCharacters NDCharacters;

    private void Awake()
    {

        HostBtn.onClick.AddListener(() =>
        {
            NDCharacters.Character = Character.Captain;
            if (IP.text != "")
            {
                GetComponent<UnityTransport>().SetConnectionData(IP.text, 7777, "0.0.0.0");
                PlayerPrefs.SetString("IP", IP.text);
            }
            else
            {
                if (PlayerPrefs.GetString("IP") != null)
                    GetComponent<UnityTransport>().SetConnectionData(PlayerPrefs.GetString("IP"), 7777, "0.0.0.0");
                else
                    PlayerPrefs.SetString("IP", GetComponent<UnityTransport>().ConnectionData.Address);
            }
                
            NetworkManager.Singleton.StartHost();
            Host.SetActive(true);
            Client.SetActive(false);
            HostBtn.transform.parent.gameObject.SetActive(false);
        });

        Client1Btn.onClick.AddListener(() =>
        {
            NDCharacters.Character = Character.Sailor1;
            ClientButton();
            Sailor1.SetActive(true);
            Sailor2.SetActive(false);
            Sailor1Head.SetActive(false);
            Sailor2Head.SetActive(true);
            Client1Btn.transform.parent.gameObject.SetActive(false);
        });

        Client2Btn.onClick.AddListener(() =>
        {
            NDCharacters.Character = Character.Sailor2;
            ClientButton();
            Sailor1.SetActive(false);
            Sailor2.SetActive(true);
            Sailor1Head.SetActive(true);
            Sailor2Head.SetActive(false);
            Client2Btn.transform.parent.gameObject.SetActive(false);
        });
    }

    void ClientButton()
    {
        NetworkManager.Singleton.StartClient();
        Host.SetActive(false);
        Client.SetActive(true);
    }
}
