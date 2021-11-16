using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMicStuff : MonoBehaviour
{
    AudioClip mic;
    AudioSource audsrc;
    const int FREQ = 44100;
    float[] outvals;

    // Start is called before the first frame update
    void Start()
    {
        // string micname = "front:CARD=Microphone,DEV=0";
        // foreach (string devname in Microphone.devices)
        // {
        //     // if (devname == micname)
        //     // {
        //     int minFreq, maxFreq;
        //     Microphone.GetDeviceCaps(devname, out minFreq, out maxFreq);
        //     Debug.Log(string.Format("Name: {0}, ({1} - {2})", devname, minFreq, maxFreq));

        //     // Debug.Log("Found it!");
        //     // }
        // }
        mic = Microphone.Start("", true, 999, FREQ);
        audsrc = GetComponent<AudioSource>();
        audsrc.clip = mic;
        audsrc.loop = true;
        audsrc.Play();

        EQMesh eqm = FindObjectOfType<EQMesh>();
        outvals = eqm.getEQValArray();
    }

    // Update is called once per frame
    void Update()
    {
        audsrc.GetSpectrumData(outvals, 0, FFTWindow.Rectangular);
        // float sum = 0;
        // foreach (float f in outvals)
        // {
        //     sum += f;
        // }
        // Debug.Log(sum);
    }
}
