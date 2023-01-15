using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button Client1Btn;
    [SerializeField] private Button Client2Btn;

    [SerializeField] private GameObject Host;
    [SerializeField] private GameObject Client;

    [SerializeField] private GameObject Sailor1;
    [SerializeField] private GameObject Sailor2;
    [SerializeField] private GameObject Sailor1Head;
    [SerializeField] private GameObject Sailor2Head;

    [SerializeField] private TextMeshProUGUI IPText;

    [SerializeField] private NetworkDataCharacters NDCharacters;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            GetComponent<UnityTransport>().ConnectionData.Address = IPText.text;
            NDCharacters.Character = Character.Captain;
            NetworkManager.Singleton.StartHost();
            Host.SetActive(true);
            Client.SetActive(false);
            HostBtn.transform.parent.gameObject.SetActive(false);
        });

        Client1Btn.onClick.AddListener(() =>
        {
            NDCharacters.Character = Character.Sailor1;
            GetComponent<UnityTransport>().ConnectionData.Address = IPText.text;
            ClientButton();
            Sailor1.SetActive(true);
            Sailor2.SetActive(false);
            Sailor1Head.SetActive(false);
            Sailor2Head.SetActive(true);
            Client1Btn.transform.parent.gameObject.SetActive(false);
        });

        Client2Btn.onClick.AddListener(() =>
        {
            GetComponent<UnityTransport>().ConnectionData.Address = IPText.text;
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
