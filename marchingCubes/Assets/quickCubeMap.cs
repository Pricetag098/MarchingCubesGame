using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickCubeMap : MonoBehaviour
{
    bool[,,] cubeMap;
    public int xSize, ySize, zSize;
    public float cubeSize;
    public Vector3 offset = new Vector3(0,0,0);
    [Range(-1, 1)]
    public float toggleThreshold = .5f;

    public CubeGen cubeGen;
    [Space(10)]

    public float scale = 1;
    public float frequncy = 1;

    [Space(10)]

    public bool genMesh = false;

    public bool debugMesh = false;

    public bool[,,] debug = new bool[2,2,2];

    public bool[] debugPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (genMesh)
        {
			Vector3 pos = transform.position;
			float off = cubeSize * .94f;
			offset = new Vector3((pos.x / 16) * off, (pos.y / 16) * off, (pos.z / 16) * off);
			GenMap(xSize, ySize, zSize);
            
            
            cubeGen.GenerateMesh(cubeMap, cubeSize);
            genMesh = false;
        }
        if (debugMesh)
        {
            createDebug();

            cubeGen.GenerateMesh(debug, cubeSize);
            //debugMesh = false;
        }
    }

    void createDebug()
    {
        debug[0, 0, 0] = debugPoints[0];
        debug[1, 0, 0] = debugPoints[1];
        debug[1, 0, 1] = debugPoints[2];
        debug[0, 0, 1] = debugPoints[3];
        debug[0, 1, 0] = debugPoints[4];
        debug[1, 1, 0] = debugPoints[5];
        debug[1, 1, 1] = debugPoints[6];
        debug[0, 1, 1] = debugPoints[7];
    }
    public void GenMap(int xSize, int ySize, int zSize)
    {
        cubeMap = new bool[xSize,ySize,zSize];
        float min = float.MaxValue;
        float Max = float.MinValue;
        //float[,,] floatMap = new float[xSize, ySize, zSize];

        for(int x = 0;x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    float xCoord = (((float)x / xSize) / scale * frequncy)+ offset.x;
                    float yCoord = (((float)y / ySize) / scale * frequncy) + offset.y;
                    float zCoord = (((float)z / zSize) / scale * frequncy) + offset.z;
                    float perlinVal = Perlin.Noise(xCoord, yCoord, zCoord);
                    //Debug.Log(perlinVal);
                    
                    if (perlinVal < min)
                    {
                        min = perlinVal;
                    }
                    if(perlinVal > Max)
                    {
                        Max = perlinVal;
                    }
                    cubeMap[x, y, z] = perlinVal > toggleThreshold;
                }
            }
        }

        //Debug.Log("min: " + min + " Max: " + Max);

    }
}
