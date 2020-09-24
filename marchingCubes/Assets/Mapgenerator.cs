using System.Collections;
using System;
using UnityEngine;


public class Mapgenerator : MonoBehaviour
{
    public int width, height;

    [Range(0,100)]
    public int randomFillPercent;

    public int maxWalls = 4;
    public int smoothIterations = 5;

    public string seed;
    public bool useRandomSeed;

    public float squareSize = 1;

    int[,] map;

    private void Start()
    {
        GenerateMap();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();
        for (int i = 0; i < smoothIterations; i++)
        {
            smoothMap();
            
        }
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(map, squareSize);
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random prng = new System.Random(seed.GetHashCode());

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y< height; y++)
            {
                if(x == 0 || x == width -1 || y==0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
                
            }
        }
    }

    void smoothMap()
    {
        for (int x = 0; x < width -1; x++)
        {
            for (int y = 0; y < height -1; y++)
            {
                int nWallCount = GetSurroundingWallCount(x, y);
                if (nWallCount > maxWalls)
                {
                    map[x, y] = 1;
                }
                else if (nWallCount < maxWalls)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int nX = gridX -1; nX <= gridX + 1; nX++)
        {
            for (int nY = gridY - 1; nY <= gridY + 1; nY++)
            {
                if(nX >= 0 && nX<=width && nY >= 0 && nY <= height)
                {
                    if(nX != gridX || nY != gridY)
                    {
                        wallCount += map[nX, nY];
                    }
                }
                else
                {
                    wallCount++;
                }
                
            }
        }
        return wallCount;
    }
    /*
    private void OnDrawGizmos()
    {
        if(map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector3 position = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(position, Vector3.one);
                }
            }
        }
        
    }
    */
}
