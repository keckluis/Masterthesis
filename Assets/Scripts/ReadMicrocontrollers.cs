using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ReadMicrocontrollers : MonoBehaviour
{
    List<SerialPort> SPs = new List<SerialPort>();
    List<Microcontroller> MicroControllers = new List<Microcontroller>();

    public SailsManager SailsManager;
    public float Wheel = 0f, Sheet = 0f, Halyard = 0f;

    bool FoundAllMicrocontrollers = false;

    void Start()
    {
        foreach (string name in SerialPort.GetPortNames())
        {
            if (name != "COM1")
            {
                Debug.Log("Found " + name);
                SPs.Add(new SerialPort(name, 9600));
            }
        }
    }

    void Update()
    {
        if (!FoundAllMicrocontrollers)
        {
            foreach (SerialPort sp in SPs)
            {
                LookForMicroController(sp);
            }
        }
        else
        {
            foreach(Microcontroller mc in MicroControllers)
            {
                if (!mc.SerialPort.IsOpen)
                    mc.SerialPort.Open();
                try
                {
                    string input = mc.SerialPort.ReadLine();
                    string value = input.Remove(0, 1);

                    mc.Value = float.Parse(value) - mc.Offset;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                switch(mc.Name)
                {
                    case 'W':
                        Wheel= mc.Value; 
                        break;
                    case 'S':
                        Sheet = mc.Value;
                        break;
                    case 'H':
                        Halyard = mc.Value;
                        break;
                    default: 
                        break;
                }
            }
        }
    }

    void LookForMicroController(SerialPort sp)
    {
        try
        {
            if (!sp.IsOpen)
                sp.Open();

            string input = sp.ReadLine();
            string value = input.Remove(0, 1);

            if (input[0] == 'W' || input[0] == 'S' || input[0] == 'H')
            {
                bool alreadyFound = false;
                foreach (Microcontroller mc in MicroControllers)
                {
                    if (mc.Name == input[0])
                        alreadyFound = true;
                }

                if (!alreadyFound)
                {
                    MicroControllers.Add(new Microcontroller(input[0], sp, 0f, float.Parse(value)));
                }

                if (MicroControllers.Count == 3)
                {
                    FoundAllMicrocontrollers = true;
                }
            }
            sp.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
           
public class Microcontroller
{
    public char Name;
    public SerialPort SerialPort;
    public float Value;
    public float Offset;

    public Microcontroller(char name, SerialPort serialPort, float value, float offset)
    {
        Name = name;
        SerialPort = serialPort;
        Value = value;
        Offset = offset;
    }
}
