using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;

    public void GenerateMesh(int[,] map , float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
    }

    private void OnDrawGizmos()
    {
        if(squareGrid != null)
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    Gizmos.color = squareGrid.squares[x, y].topLeft.active ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topLeft.pos, Vector3.one * .4f);

                    Gizmos.color = squareGrid.squares[x, y].topRight.active ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].topRight.pos, Vector3.one * .4f);

                    Gizmos.color = squareGrid.squares[x, y].bottomLeft.active ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomLeft.pos, Vector3.one * .4f);

                    Gizmos.color = squareGrid.squares[x, y].bottomRight.active ? Color.white : Color.black;
                    Gizmos.DrawCube(squareGrid.squares[x, y].bottomRight.pos, Vector3.one * .4f);

                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreTop.pos, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreBottom.pos, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreLeft.pos, Vector3.one * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x, y].centreRight.pos, Vector3.one * .15f);
                }
            }
        }
    }
    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControllNode[,] controllNodes = new ControllNode[nodeCountX, nodeCountY];

            for(int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controllNodes[x, y] = new ControllNode(pos, map[x, y] == 1, squareSize);
                }
            }
            squares = new Square[nodeCountX - 1, nodeCountY - 1];

            for (int x = 0; x < nodeCountX -1; x++)
            {
                for (int y = 0; y < nodeCountY -1; y++)
                {
                    squares[x, y] = new Square(controllNodes[x, y + 1], controllNodes[x + 1, y + 1], controllNodes[x + 1, y], controllNodes[x, y]);
                }
            }
        }
    }

    public class Square
    {
        public ControllNode topRight, topLeft, bottomRight, bottomLeft;
        public Node centreTop, centreRight, centreLeft, centreBottom;

        public Square(ControllNode _topRight, ControllNode _topLeft, ControllNode _bottomRight, ControllNode _bottomLeft)
        {
            topRight = _topRight;
            topLeft = _topLeft;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

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

    public class ControllNode : Node
    {
        public bool active;
        public Node above, right;

        public ControllNode(Vector3 _pos, bool _active, float squareSize): base(_pos)
        {
            active = _active;

            above = new Node(pos + Vector3.forward * squareSize / 2f);
            right = new Node(pos + Vector3.right * squareSize / 2f);
        }
    }
}
