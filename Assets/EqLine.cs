using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqLine : MonoBehaviour
{
    [Range(1, 32)]
    public int lineResolution = 10;
    public float mvtAmt = 20f;
    [Range(0f, 1f)]
    public float xbendFactor = 0.5f;
    public float relaxFactor = 1.5f;

    LineRenderer lr;
    MyMicStuff myMicStuff;
    int numVtxs;
    float[] eqVals;
    int numEqVals;
    float[] ampCorrection;
    float[] eqValMemory;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        myMicStuff = FindObjectOfType<MyMicStuff>();
        eqVals = myMicStuff.getEqVals();
        eqValMemory = new float[eqVals.Length];
        numVtxs = eqVals.Length * lineResolution + 2;
        lr.positionCount = numVtxs;
        for (int i = 0; i < numVtxs; i++)
        {
            float t = ((float)i) / ((float)(numVtxs - 1));
            lr.SetPosition(i, new Vector3(t, 0, 0));
        }
        numEqVals = eqVals.Length;

        ampCorrection = new float[eqVals.Length];
        for (int i = 0; i < eqVals.Length; i++)
        {
            ampCorrection[i] = Mathf.Pow(2f, 5f * ((float)i) / ((float)eqVals.Length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        int numInteriorPts = (numEqVals - 1) * lineResolution;
        int i1 = 0;
        int eqi = 0;
        int i2 = Mathf.CeilToInt(((float)lineResolution) / 2f);
        float v1 = 0;
        float v2 = eqVals[0] * ampCorrection[0] * mvtAmt;
        if (v2 > eqValMemory[0])
        {
            eqValMemory[0] = v2;
        }
        else
        {
            v2 = Mathf.Lerp(eqValMemory[0], 0, Time.deltaTime * relaxFactor);
            eqValMemory[0] = v2;
        }

        for (int i = 0; i < numVtxs; i++)
        {
            if (i > i2)
            {
                i1 = i2;
                eqi++;
                v1 = v2;
                i2 += lineResolution;
                if (eqi >= eqVals.Length)
                {
                    i2 = numVtxs - 1;
                    v2 = 0;
                }
                else
                {
                    v2 = eqVals[eqi] * ampCorrection[eqi] * mvtAmt;
                    if (v2 > eqValMemory[eqi])
                    {
                        eqValMemory[eqi] = v2;
                    }
                    else
                    {
                        v2 = Mathf.Lerp(eqValMemory[eqi], 0, Time.deltaTime * relaxFactor);
                        eqValMemory[eqi] = v2;
                    }
                }
            }
            Vector3 p = lr.GetPosition(i);
            float t = ((float)i) / ((float)(numVtxs - 1));
            p.x = Mathf.Lerp(1f - Mathf.Pow(0.5f, ((float)i) / ((float)lineResolution)), t, xbendFactor);
            p.y = Mathf.Log(Mathf.SmoothStep(v1, v2, Mathf.InverseLerp(i1, i2, i)) + 0.1f) + 4f;
            lr.SetPosition(i, p);
        }
    }
}
