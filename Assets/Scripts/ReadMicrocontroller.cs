using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ReadMicrocontroller : MonoBehaviour
{
    SerialPort[] SPs = new SerialPort[3];

    public float Wheel = 0f, Sheet = 0f, Halyard = 0f;

    void Start()
    {
        int i = 0;
        foreach(string name in SerialPort.GetPortNames())
        {
            if (name != "COM1" && i < SPs.Length)
            {
                SPs[i] = new SerialPort("\\\\.\\" + name, 9600);
                SPs[i].Open();
                i++;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < SPs.Length; i++)
        {
            if (SPs[i] != null)
            {
                if (!SPs[i].IsOpen)
                    SPs[i].Open();

                string input = SPs[i].ReadLine();
                string value = input.Remove(0, 1);
                switch (input[0])
                {
                    case 'W':
                        Wheel = float.Parse(value);
                        break;

                    case 'S':
                        Sheet = float.Parse(value);
                        break;

                    case 'H':
                        Halyard = float.Parse(value);
                        break;

                    default:
                        break;
                }

            }
        }
    }
}
