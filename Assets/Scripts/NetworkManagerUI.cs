using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button Client1Btn;
    [SerializeField] private Button Client2Btn;

    [SerializeField] private GameObject Host;
    [SerializeField] private GameObject Client;

    [SerializeField] private GameObject Sailor1;
    [SerializeField] private GameObject Sailor2;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Host.SetActive(true);
            Client.SetActive(false);
            HostBtn.transform.parent.gameObject.SetActive(false);
        });

        Client1Btn.onClick.AddListener(() =>
        {
            ClientButton();
            Sailor1.SetActive(true);
            Sailor2.SetActive(false);
            Client1Btn.transform.parent.gameObject.SetActive(false);
        });

        Client2Btn.onClick.AddListener(() =>
        {
            ClientButton();
            Sailor1.SetActive(false);
            Sailor2.SetActive(true);
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
