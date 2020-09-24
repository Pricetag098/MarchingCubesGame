using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CubeGen : MonoBehaviour
{
    [Range(0, 7)]
    public int check;
    [Range(0, 11)]
    public int check2;
    public CubeGrid cubeGrid;
    public MeshFilter meshFilter;
    public Mesh mesh;
    public void GenerateMesh(bool[,,] map, float cubeSize)
    {
        cubeGrid = new CubeGrid(map, cubeSize);
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        genMesh();
    }

    public class CubeGrid
    {
        public Cube[,,] cubes;

        public CubeGrid(bool[,,] map,float cubeSize){
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            int nodeCountZ = map.GetLength(2);
            float mapWidth = nodeCountX * cubeSize;
            float mapHeight = nodeCountY * cubeSize;
            float mapDepth = nodeCountZ * cubeSize;

            ControlNode[,,] controlNodes = new ControlNode[nodeCountX, nodeCountY, nodeCountZ];

            for(int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    for (int z = 0; z < nodeCountZ; z++)
                    {
                        Vector3 pos = new Vector3(-mapWidth / 2 + x * cubeSize + cubeSize / 2, -mapHeight / 2 + y * cubeSize + cubeSize / 2, -mapDepth / 2 + z * cubeSize + cubeSize / 2);
                        controlNodes[x, y,z] = new ControlNode(pos, map[x, y,z], cubeSize);
                    }
                }
            }

            cubes = new Cube[nodeCountX - 1, nodeCountY - 1, nodeCountZ - 1];

            //Debug.Log(controlNodes.GetLength(1));
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    for (int z = 0; z < nodeCountZ - 1; z++)
                    {
                        ControlNode[] cubeNodes = new ControlNode[8];
                        cubeNodes[0] = controlNodes[x, y, z];
                        cubeNodes[1] = controlNodes[x, y, z+1];
                        cubeNodes[2] = controlNodes[x + 1, y, z + 1];
                        cubeNodes[3] = controlNodes[x+1, y, z];
                        cubeNodes[4] = controlNodes[x, y + 1, z];
                        cubeNodes[5] = controlNodes[x, y + 1, z + 1];
                        cubeNodes[6] = controlNodes[x + 1, y + 1, z +1];
                        cubeNodes[7] = controlNodes[x + 1, y + 1, z];

                        

                        cubes[x, y, z] = new Cube(cubeNodes);
                        //Debug.Log(cubes[x, y, z]);
                    }
                }
            }
            
        }

    }

    public class Node
    {
        public Vector3 pos;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            pos = _pos;
        }
    }

    public class ControlNode : Node
    {
        public bool active;
        public Node above, right, front;

        public ControlNode(Vector3 _pos, bool _active, float cubeSize) : base(_pos)
        {
            active = _active;

            above = new Node(pos + Vector3.up * cubeSize / 2f);
            right = new Node(pos + Vector3.right * cubeSize / 2f);
            front = new Node(pos + Vector3.forward * cubeSize / 2f);
        }
    }

    public class Cube
    {
        public ControlNode[] controlNodes = new ControlNode[7];
        public Node[] subNodes = new Node[12];
        public int config = 0;
        
        public Cube(ControlNode[] _controlNodes)
        {
            controlNodes = _controlNodes;

            /*
            subNodes[0] = controlNodes[0].right;
            subNodes[1] = controlNodes[0].right;
            subNodes[2] = controlNodes[0].right;
            subNodes[3] = controlNodes[0].right;
            subNodes[4] = controlNodes[0].right;
            subNodes[5] = controlNodes[0].right;
            subNodes[6] = controlNodes[0].right;
            subNodes[7] = controlNodes[0].right;
            subNodes[8] = controlNodes[0].right;
            subNodes[9] = controlNodes[0].right;
            subNodes[10] = controlNodes[0].right;
            subNodes[11] = controlNodes[0].right;
            */

            
            subNodes[0] = controlNodes[0].front;
            subNodes[1] = controlNodes[1].right;
            subNodes[2] = controlNodes[3].front;
            subNodes[3] = controlNodes[0].right;
            subNodes[4] = controlNodes[4].front;
            subNodes[5] = controlNodes[5].right;
            subNodes[6] = controlNodes[7].front;
            subNodes[7] = controlNodes[4].right;
            subNodes[8] = controlNodes[0].above;
            subNodes[9] = controlNodes[1].above;
            subNodes[10] = controlNodes[2].above;
            subNodes[11] = controlNodes[3].above;
            
            if (controlNodes[0].active)
            {
                config += 1;
            }
            if (controlNodes[1].active)
            {
                config += 2;
            }
            if (controlNodes[2].active)
            {
                config += 4;
            }
            if (controlNodes[3].active)
            {
                config += 8;
            }
            if (controlNodes[4].active)
            {
                config += 16;
            }
            if (controlNodes[5].active)
            {
                config += 32;
            }
            if (controlNodes[6].active)
            {
                config += 64;
            }
            if (controlNodes[7].active)
            {
                config += 128;
            }
            
        }
    }

    public void genMesh()
    {
        mesh.Clear();
        List <Vector3> vertList = new List<Vector3>();
		//Vector3[] vertlist = new Vector3[cubeGrid.cubes.GetLength(0) * cubeGrid.cubes.GetLength(1) * cubeGrid.cubes.GetLength(2) * 12];
		List<int> triList = new List<int>();
		//int cubeNumber = 0;
		int cubeNum = 0;
		for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
            {
                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
                {
                    for (int c = 0; c < 12; c++)
                    {
                        //Debug.Log(cubeGrid.cubes[x, y, z].subNodes[c].pos);
                        vertList.Add(cubeGrid.cubes[x, y, z].subNodes[c].pos);
                        //vertlist[cubeNumber + c] = cubeGrid.cubes[x, y, z].subNodes[c].pos;
                    }
					for (int i = 0; TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i] != -1; i += 3)
					{
						//Debug.Log(TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config,i]);
						triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i]) + cubeNum * 12);
						triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i + 1]) + cubeNum * 12);
						triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i + 2]) + cubeNum * 12);
						//yield return new WaitForSecondsRealtime(0);
					}
					cubeNum++;
				}
            }
        }
        mesh.vertices = vertList.ToArray();

        
        /*
        for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
            {
                for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
                {
                    for (int i = 0; TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i] != -1; i += 3)
                    {
                        //Debug.Log(TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config,i]);
                        triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i]) + cubeNum * 12);
                        triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i + 1]) + cubeNum * 12);
                        triList.Add((TriangeInfo.triTable[cubeGrid.cubes[x, y, z].config, i + 2]) + cubeNum *12);
                        //yield return new WaitForSecondsRealtime(0);
                    }
                    cubeNum++;
                }
            }
        }
		*/
        mesh.triangles = triList.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.mesh = mesh;
        //
    }
    /*
    private void OnDrawGizmos()
    {
        
        if (cubeGrid != null)
        {
            //Debug.Log(cubeGrid.cubes.GetLength(0));
            //Debug.Log(cubeGrid.cubes.GetLength(1));
            //Debug.Log(cubeGrid.cubes.GetLength(2));

            //Debug.Log("ahfuk" + cubeGrid.cubes[0, 0, 0] + "CUNT");
            for (int x = 0; x < cubeGrid.cubes.GetLength(0); x++)
            {
                for (int y = 0; y < cubeGrid.cubes.GetLength(1); y++)
                {
                    for (int z = 0; z < cubeGrid.cubes.GetLength(2); z++)
                    {

                        //Debug.Log(cubeGrid.cubes[x,y,z].config);
                        //Debug.Log(x.ToString() + y.ToString() + z.ToString() +cubeGrid.cubes[x, y, z]);
                        //Debug.Log(cubeGrid.cubes[x, y, z].controlNodes);
                        for (int i = 0; i < cubeGrid.cubes[x, y, z].controlNodes.GetLength(0); i++)
                        {
                            
                                Gizmos.color = (cubeGrid.cubes[x, y, z].controlNodes[i].active) ? Color.black : Color.white;
                                Gizmos.DrawSphere(cubeGrid.cubes[x, y, z].controlNodes[i].pos, .1f);
                           
                            

                        }

                    }
                }
            }
            

        }
    } 
    */
}
