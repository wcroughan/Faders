using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AValHandle : MonoBehaviour
{
    private Dictionary<string, Vector3> movementAxis;
    private Dictionary<string, Vector3> rotationAxis;
    private Dictionary<string, float> rotationAmt;
    private Dictionary<string, float> currentVals;

    private const int BEHAVIOR_ROTATE = 0;
    private const int BEHAVIOR_MOVE = 1;

    private Vector3 nextUpdateTransform;
    private Vector3 nextUpdateRotation;

    public void SetValue(string key, float val)
    {
        float dval = val - currentVals[key];
        currentVals[key] = val;

        nextUpdateTransform += movementAxis[key] * dval;
        nextUpdateRotation += rotationAxis[key] * rotationAmt[key] * dval;
    }

    void Awake()
    {
        movementAxis = new Dictionary<string, Vector3>();
        currentVals = new Dictionary<string, float>();
        rotationAxis = new Dictionary<string, Vector3>();
        rotationAmt = new Dictionary<string, float>();
        nextUpdateTransform = Vector3.zero;
        nextUpdateRotation = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        movementAxis["testkey"] = new Vector3(10f, 0f, 0f);
        currentVals["testkey"] = 0.5f;
        rotationAxis["testkey"] = new Vector3(0f, 0f, -1f);
        rotationAmt["testkey"] = 0f;
        // movementAxis["testkey2"] = new Vector3(0f, 0f, 0f);
        // currentVals["testkey2"] = 0.5f;
        // rotationAxis["testkey2"] = new Vector3(0f, 0f, -1f);
        // rotationAmt["testkey2"] = 270f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(nextUpdateTransform, Space.World);
        nextUpdateTransform = Vector3.zero;
        transform.Rotate(nextUpdateRotation, Space.World);
        nextUpdateRotation = Vector3.zero;
    }

    public void AddAVal(string key, float currentVal, Vector3 movement, Vector3 rotAxis, float rotationRange)
    {
        currentVals[key] = currentVal;
        movementAxis[key] = movement;
        rotationAxis[key] = rotAxis;
        rotationAmt[key] = rotationRange;
    }

    public bool HasKey(string key)
    {
        return currentVals.ContainsKey(key);
    }
}
