using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AValsManager : MonoBehaviour
{
    private List<AValHandle> handles;

    public void SetAVal(string key, float val)
    {
        foreach (AValHandle h in handles)
        {
            if (h.HasKey(key))
                h.SetValue(key, val);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        handles = new List<AValHandle>(FindObjectsOfType<AValHandle>());
        Debug.Log(handles[0]);
        handles[0].AddAVal("testkey2", 0.5f, new Vector3(0, 0, 0), new Vector3(0, 0, 1), 270f);
        handles[1].AddAVal("testkey3", 0.5f, new Vector3(0, 0, 0), new Vector3(0, 0, 1), 270f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
