using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqLine : MonoBehaviour
{
    [Range(1, 32)]
    public int lineResolution = 10;
    public float mvtAmt = 20f;

    LineRenderer lr;
    MyMicStuff myMicStuff;
    int numVtxs;
    float[] eqVals;
    int numEqVals;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        myMicStuff = FindObjectOfType<MyMicStuff>();
        eqVals = myMicStuff.getEqVals();
        numVtxs = eqVals.Length * lineResolution + 2;
        lr.positionCount = numVtxs;
        for (int i = 0; i < numVtxs; i++)
        {
            float t = ((float)i) / ((float)(numVtxs - 1));
            lr.SetPosition(i, new Vector3(t, 0, 0));
        }
        numEqVals = eqVals.Length;
    }

    // Update is called once per frame
    void Update()
    {
        int numInteriorPts = (numEqVals - 1) * lineResolution;
        for (int i = 0; i < numVtxs; i++)
        {
            float t = ((float)i) / ((float)(numVtxs - 1));
            lr.SetPosition(i, new Vector3(t, eqVals[i % eqVals.Length] * mvtAmt, 0));
        }
    }
}
