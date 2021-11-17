using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MyMicStuff : MonoBehaviour
{
    [Range(6, 13)]
    public int eqResolution = 6;

    AudioClip mic;
    AudioSource audsrc;
    const int FREQ = 44100;
    float[] eqVals;
    int numEqVals;

    string outfilename;
    StreamWriter sw;

    void Awake()
    {
        numEqVals = (int)Mathf.Pow(2.0f, (float)eqResolution);
        eqVals = new float[numEqVals];
        outfilename = "Assets/eqvals.txt";
    }

    // Start is called before the first frame update
    void Start()
    {
        sw = new StreamWriter(outfilename, false);

        string micname = "front:CARD=Microphone,DEV=0";
        foreach (string devname in Microphone.devices)
        {
            // if (devname == micname)
            // {
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devname, out minFreq, out maxFreq);
            Debug.Log(string.Format("Name: {0}, ({1} - {2})", devname, minFreq, maxFreq));
            if (devname.Contains("Monitor") && devname.Contains("Built-in"))
                micname = devname;

            // Debug.Log("Found it!");
            // }
        }
        mic = Microphone.Start(micname, true, 999, FREQ);
        audsrc = GetComponent<AudioSource>();
        audsrc.clip = mic;
        audsrc.loop = true;
        // audsrc.mute = true;
        audsrc.Play();
    }

    void OnApplicationQuit()
    {
        sw.Close();
    }

    public float[] getEqVals()
    {
        return eqVals;
    }

    public int getEqResolution()
    {
        return eqResolution;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audsrc.GetSpectrumData(eqVals, 0, FFTWindow.Rectangular);

        // sw.WriteLine(string.Join(",", eqVals));
        // float sum = 0;
        // foreach (float f in outvals)
        // {
        //     sum += f;
        // }
        // Debug.Log(sum);
    }
}
