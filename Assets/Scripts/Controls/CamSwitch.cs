using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] Cams;
    private int current = 0;

    private void Start()
    {
        Cams[0].SetActive(true);
        for (int i = 1; i < Cams.Length; i++)
        {
            Cams[i].SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cams[current].SetActive(false);

            if (current + 1 == Cams.Length)
            {
                current = 0;
            }
            else
                current += 1;
            Cams[current].SetActive(true);
        }
    }
}
