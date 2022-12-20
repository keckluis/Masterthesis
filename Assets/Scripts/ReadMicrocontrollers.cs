using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ReadMicrocontrollers : MonoBehaviour
{
    List<SerialPort> SPs = new List<SerialPort>();
    List<Microcontroller> MicroControllers = new List<Microcontroller>();

    public SailsManager SailsManager;
    public RudderControls RudderControls;
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

        foreach(SerialPort sp in SPs)
        {
            sp.ReadTimeout = 1;
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
        
        foreach(Microcontroller mc in MicroControllers)
        {
            if (!mc.SerialPort.IsOpen)
                mc.SerialPort.Open();
            try
            {
                string input = mc.SerialPort.ReadLine();
                string value = input.Remove(0, 1);

                mc.Value = float.Parse(value);
            }
            catch (TimeoutException e) {}

            switch(mc.Name)
            {
                case 'W':
                    
                    if (mc.Value > 6000f)
                    {
                        if (mc.Value - 6000f > mc.OffsetPos)
                            mc.OffsetPos = mc.Value - 6000f;
                        Wheel = mc.Value - mc.OffsetPos;
                    }
                    else if (mc.Value < -6000f)
                    {
                        if (mc.Value + 6000f < mc.OffsetNeg)
                            mc.OffsetNeg = mc.Value + 6000f;
                        Wheel = mc.Value - mc.OffsetNeg;
                    }
                    else 
                    {
                        Wheel = mc.Value;  
                        mc.OffsetNeg = 0f;
                        mc.OffsetPos = 0f; 
                    }

                    RudderControls.Degrees = -((Wheel / 6000f) * 179f);
                        
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
                    MicroControllers.Add(new Microcontroller(input[0], sp, 0f, 0f, 0f));
                    Debug.Log(sp.PortName + " is input " + input[0]);
                }

                if (MicroControllers.Count == 3)
                {
                    FoundAllMicrocontrollers = true;
                    return;
                }
            }
            else
                sp.Close();
            
            sp.Close();
        }
        catch (TimeoutException e) { }
    }
}
           
public class Microcontroller
{
    public char Name;
    public SerialPort SerialPort;
    public float Value;
    public float OffsetPos;
    public float OffsetNeg;

    public Microcontroller(char name, SerialPort serialPort, float value, float offsetPos, float offsetNeg)
    {
        Name = name;
        SerialPort = serialPort;
        Value = value;
        OffsetPos = offsetPos;
        OffsetNeg = offsetNeg;
    }
}
