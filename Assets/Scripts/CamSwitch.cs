using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject MainCam;
    public GameObject WheelCam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            MainCam.SetActive(!MainCam.activeSelf);
            WheelCam.SetActive(!WheelCam.activeSelf);
        }
    }
}
