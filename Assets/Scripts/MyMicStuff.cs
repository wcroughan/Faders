using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;

public class MyMicStuff : MonoBehaviour
{
    [Range(6, 13)]
    public int eqResolution = 6;
    public bool outputToText = false;
    public AudioMixer mixer;

    AudioClip mic;
    List<AudioSource> audsrc;
    const int FREQ = 44100;
    float[] eqVals;
    float[] tmpVals;
    int numEqVals;

    string outfilename;
    StreamWriter sw;

    void Awake()
    {
        numEqVals = (int)Mathf.Pow(2.0f, (float)eqResolution);
        eqVals = new float[numEqVals];
        tmpVals = new float[numEqVals];
        outfilename = "Assets/eqvals.txt";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (outputToText)
            sw = new StreamWriter(outfilename, false);

        audsrc = new List<AudioSource>();
        // mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        Debug.Log(mixer);
        AudioMixerGroup[] groups = mixer.FindMatchingGroups("Master");
        AudioMixerGroup nullGroup = groups[0];
        foreach (string devname in Microphone.devices)
        {
            if (devname.Contains("Monitor"))
            {
                Debug.Log(string.Format("Connecting to audio device {0}", devname));
                AudioSource a = gameObject.AddComponent<AudioSource>();
                a.clip = Microphone.Start(devname, true, 999, FREQ);
                a.loop = true;
                a.outputAudioMixerGroup = nullGroup;
                a.Play();
                audsrc.Add(a);
            }
            else
            {
                Debug.Log(string.Format("Skipping dev {0}", devname));
            }
        }
    }

    void OnApplicationQuit()
    {
        if (outputToText)
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

        for (int i = 0; i < eqVals.Length; i++)
            eqVals[i] = 0;

        foreach (AudioSource a in audsrc)
        {
            a.GetSpectrumData(tmpVals, 0, FFTWindow.Rectangular);
            for (int i = 0; i < eqVals.Length; i++)
                eqVals[i] += tmpVals[i];
        }

        if (outputToText)
            sw.WriteLine(string.Join(",", eqVals));

    }
}
