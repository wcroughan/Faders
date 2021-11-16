using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EQMesh : MonoBehaviour
{
    [Range(1, 10)]
    public int meshResolution = 1;
    [Range(6, 13)]
    public int eqResolution = 6;
    [Range(0.1f, 3f)]
    public float radiusInner = 1.5f;
    [Range(0.1f, 3f)]
    public float thickness = 0.5f;
    [Range(0.1f, 3f)]
    public float movementAmt = 1.5f;
    public Color colorLow = Color.cyan;
    public Color colorHigh = Color.red;
    [Range(0.01f, 1.0f)]
    public float volumeColorFactor = 0.3f;
    [Range(0.001f, 0.999f)]
    public float falloffFactor = 0.1f;
    public float rotationSpeed = 1f;
    public float rotationSlowFactor = 1f;
    public float rotationResponsiveness = 1f;
    public float freqBandEqualizeFactor = 500f;

    float[] eqVals; //min len 64, max len 8192
    MeshFilter mf;
    int numRingSegments;
    int numEqVals;
    float radiusOuter;
    int numFreqBands;
    float[] freqBandPower;
    Material mat;
    float currentRotationSpeed;
    float[] segmentHeight;

    void Awake()
    {
        numEqVals = (int)Mathf.Pow(2.0f, (float)eqResolution);
        eqVals = new float[numEqVals];
        radiusOuter = radiusInner + thickness;
        numFreqBands = eqResolution + 1;
        freqBandPower = new float[numFreqBands];
        numRingSegments = meshResolution * numFreqBands;
        currentRotationSpeed = rotationSpeed;
        segmentHeight = new float[numRingSegments + 1];
    }

    // Start is called before the first frame update
    void Start()
    {
        mf = GetComponent<MeshFilter>();

        const float TAU = 6.28318531f;

        Mesh m = new Mesh();
        List<Vector3> pts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < numRingSegments + 1; i++)
        {
            float t = ((float)i) / ((float)numRingSegments);
            float a = t * TAU;
            float x1 = radiusInner * Mathf.Cos(a);
            float y1 = radiusInner * Mathf.Sin(a);
            float x2 = radiusOuter * Mathf.Cos(a);
            float y2 = radiusOuter * Mathf.Sin(a);
            pts.Add(new Vector3(x1, y1, 0f));
            pts.Add(new Vector3(x2, y2, 0f));
            uvs.Add(new Vector2(t, 0f));
            uvs.Add(new Vector2(t, 1f));
        }

        List<int> tridxs = new List<int>();
        for (int i = 0; i < numRingSegments; i++)
        {
            tridxs.Add(2 * i);
            tridxs.Add(2 * i + 3);
            tridxs.Add(2 * i + 1);
            tridxs.Add(2 * i);
            tridxs.Add(2 * i + 2);
            tridxs.Add(2 * i + 3);
        }


        m.SetVertices(pts);
        m.SetTriangles(tridxs, 0);
        m.SetUVs(0, uvs);
        m.RecalculateNormals();

        mf.sharedMesh = m;

        mat = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public float[] getEQValArray()
    {
        return eqVals;
    }

    // Update is called once per frame
    void Update()
    {
        float sum = 0;
        int fbi = 0;
        freqBandPower[0] = 0;
        for (int i = 0; i < numEqVals; i++)
        {
            if (i == Mathf.RoundToInt(Mathf.Pow(2f, fbi)))
            {
                fbi += 1;
                freqBandPower[fbi] = 0;
            }
            freqBandPower[fbi] += eqVals[i] * (fbi + freqBandEqualizeFactor) * freqBandEqualizeFactor;
            sum += eqVals[i];
        }
        mat.color = Color.Lerp(colorLow, colorHigh, sum * volumeColorFactor);

        Mesh m = mf.sharedMesh;
        List<Vector3> v = new List<Vector3>();
        m.GetVertices(v);

        int i1 = 0, i2 = 1;
        for (int i = 0; i < numRingSegments + 1; i++)
        {
            if (i > i2 * meshResolution)
            {
                i1 += 1;
                i2 += 1;
            }
            float t = ((float)i - i1 * meshResolution) / ((float)meshResolution);
            float h = Mathf.SmoothStep(freqBandPower[i1], freqBandPower[i2 % numFreqBands], t);
            if (h > segmentHeight[i])
            {
                segmentHeight[i] = h;
            }
            else
            {
                // float ff = Mathf.Lerp(1f, falloffFactor, Time.deltaTime);
                float ff = Mathf.Pow(falloffFactor / 1000f, Time.deltaTime);
                h = segmentHeight[i] * ff;
                segmentHeight[i] = h;
            }

            Vector3 p = v[2 * i].normalized;
            p *= radiusInner + movementAmt * h;
            v[2 * i] = p;

            p = v[2 * i + 1].normalized;
            p *= radiusOuter + movementAmt * h;
            v[2 * i + 1] = p;
        }

        m.SetVertices(v);
        mf.sharedMesh = m;

        float newRotSpeed = Mathf.Lerp(rotationSpeed, 0, sum * rotationSlowFactor);
        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, newRotSpeed, Time.deltaTime * rotationResponsiveness);
        transform.Rotate(Vector3.forward * currentRotationSpeed, Space.Self);
    }
}
