using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderHandle : MonoBehaviour
{
    public float minY = -1f;
    public float maxY = 1f;

    FaderController faderController;
    public int faderCC;
    public int faderIdx;

    private Vector3 mouseDragOffset;
    private float mzCoord;

    private float newYVal;
    private bool yvalNeedsUpdate;

    // Start is called before the first frame update
    void Start()
    {
        newYVal = minY;
        yvalNeedsUpdate = true;

        faderController = FindObjectOfType<FaderController>();
    }

    void OnMouseDown()
    {
        mzCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseDragOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = mzCoord;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    void OnMouseDrag()
    {
        Vector3 draggedFaderPos = GetMouseWorldPos() + mouseDragOffset;
        float newFaderVal = Mathf.Clamp01(Mathf.InverseLerp(minY, maxY, draggedFaderPos.y));
        faderController.SetFaderValue(faderIdx, newFaderVal);
        // SetFaderValue(newFaderVal);
    }


    // Update is called once per frame
    void Update()
    {
        if (yvalNeedsUpdate)
        {
            yvalNeedsUpdate = false;
            transform.position = new Vector3(transform.position.x, newYVal, transform.position.z);
        }
    }

    public void SetFaderValue(float val)
    {
        //val should be 0-1
        newYVal = Mathf.Lerp(minY, maxY, val);
        yvalNeedsUpdate = true;
        Debug.Log(val);
    }


}
