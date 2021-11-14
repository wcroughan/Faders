using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderController : MonoBehaviour
{

    public FaderHandle[] handles;

    public void SetFaderValue(int faderIdx, float val)
    {
        Debug.Log(faderIdx);
        Debug.Log(val);
        if (faderIdx >= handles.Length)
        {
            Debug.Log("I don't have that fader!!");
        }
        else
        {
            handles[faderIdx].SetFaderValue(val);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
