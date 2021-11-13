using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderHandle : MonoBehaviour
{
    public float minY = 0f;
    public float maxY = 1f;

    private Vector3 mouseDragOffset;
    private float mzCoord;

    // Start is called before the first frame update
    void Start()
    {

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
        Vector3 mp = GetMouseWorldPos() + mouseDragOffset;
        float newy = Mathf.Clamp(mp.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, newy, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
