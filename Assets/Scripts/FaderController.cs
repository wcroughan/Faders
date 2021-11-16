using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderController : MonoBehaviour
{

    FaderHandle[] handles;
    public int defaultFirstCC = 23;
    UdpSocket comController;

    public void SetFaderValue(int faderIdx, float val, bool informPython = true)
    {
        if (faderIdx >= handles.Length)
        {
            Debug.Log("I don't have that fader!!");
            Debug.Log(faderIdx);
            Debug.Log(val);
        }
        else
        {
            handles[faderIdx].SetFaderValue(val);

            //if any two faders have the same parameter, update them all
            int cc = handles[faderIdx].faderCC;
            for (int i = 0; i < handles.Length; i++)
            {
                if (i != faderIdx && handles[i].faderCC == cc)
                {
                    handles[i].SetFaderValue(val);
                }
            }

            if (informPython)
            {
                comController.SendData(string.Format("FC {0} {1}", faderIdx, val));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        comController = FindObjectOfType<UdpSocket>();
        handles = FindObjectsOfType<FaderHandle>();

        for (int i = 0; i < handles.Length; i++)
        {
            handles[i].faderCC = i + defaultFirstCC;
            handles[i].faderIdx = i;
            // handles[i].faderCC = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
