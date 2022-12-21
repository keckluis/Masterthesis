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
    public Transform SheetRoll;
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
        else
        {
            foreach (SerialPort sp in SPs)
            {
                sp.Close();
            }
            SPs.Clear();
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
            catch (TimeoutException) { }

            switch(mc.Name)
            {
                case 'W':
                    Wheel = CalculateInputValues(mc, 5);
                    RudderControls.Degrees = -((Wheel / 6_000f) * 179f);
                    RudderControls.SteeringWheel.localEulerAngles = new Vector3(0f, 0f, (mc.Value / 1_200f) * 360f);
                    break;

                case 'S':
                    Sheet = CalculateInputValues(mc, 10);
                    SailsManager.SheetLength = ((Sheet / 12_000f) * 40f) + 40f;
                    SailsManager.SheetLength = Mathf.Clamp(SailsManager.SheetLength, 1f, 80f);
                    SheetRoll.localEulerAngles = new Vector3((mc.Value / 1_200f) * 360f, 0f, 0f);
                    break;

                case 'H':
                    Halyard = CalculateInputValues(mc, 10);
                    SailsManager.HalyardLength = ((Halyard / 12_000f) * 45f) + 55f;
                    SailsManager.HalyardLength = Mathf.Clamp(SailsManager.HalyardLength, 10f, 100f);
                    break;

                default: 
                    break;
            }
        }
    }

    float CalculateInputValues(Microcontroller mc, int rotations)
    {
        float minmax = 1_200f * rotations;
        float output;
        if (mc.Value > minmax)
        {
            if (mc.Value - minmax > mc.OffsetPos)
                mc.OffsetPos = mc.Value - minmax;
            output = mc.Value - mc.OffsetPos;
        }
        else if (mc.Value < -minmax)
        {
            if (mc.Value + minmax < mc.OffsetNeg)
                mc.OffsetNeg = mc.Value + minmax;
            output = mc.Value - mc.OffsetNeg;
        }
        else
        {
            output = mc.Value;
            mc.OffsetNeg = 0f;
            mc.OffsetPos = 0f;
        }

        return output;
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
        }
        catch (TimeoutException) {}
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
