using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ReadMicrocontrollers : MonoBehaviour
{
    List<SerialPort> SPs = new List<SerialPort>();
    public SailsManager SailsManager;
    public float Wheel = 0f, Sheet = 0f, Halyard = 0f;
    private float WheelOffset, SheetOffset, HalyardOffset;
    private bool firstReadW = true, firstReadS = true, firstReadH = true;

    void Start()
    {
        foreach(string name in SerialPort.GetPortNames())
        {
           SPs.Add(new SerialPort("\\\\.\\" + name, 9600));
        }
    }

    void Update()
    {
        foreach(SerialPort sp in SPs)
        {
            if (sp != null)
            {
                if (!sp.IsOpen)
                    sp.Open();

                string input = sp.ReadLine();
                string value = input.Remove(0, 1);
                switch (input[0])
                {
                    case 'W':
                        Wheel = float.Parse(value);
                        if (firstReadW)
                        {
                            WheelOffset = Wheel;
                            Wheel = 0f;
                            firstReadW = false;
                        }
                        else
                        {
                            Wheel -= WheelOffset;
                        }
                        changeWheelRot();
                        break;

                    case 'S':
                        Sheet = float.Parse(value);
                        if (firstReadS)
                        {
                            SheetOffset = Sheet;
                            Sheet = 0f;
                            firstReadS = false;
                        }
                        else
                        {
                            Sheet -= SheetOffset;
                        }
                        changeSheetLength();
                        break;

                    case 'H':
                        Halyard = float.Parse(value);
                        if (firstReadH)
                        {
                            HalyardOffset = Halyard;
                            Halyard = 0f;
                            firstReadH = false;
                        }
                        else
                        {
                            Halyard -= HalyardOffset;
                        }
                        changeHalyardLength();
                        break;

                    default:
                        //sp.Close();
                        break;
                }

            }
        }
    }

    void changeWheelRot()
    {

    }

    void changeSheetLength()
    {

    }

    void changeHalyardLength()
    {

    }
}
