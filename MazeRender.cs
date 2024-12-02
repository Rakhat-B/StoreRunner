using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class MazeRender : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    public float CellSize = 1f;

    private void Start()
    {
        MazeCell[,] maze = mazeGenerator.GetMaze();

        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

                MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                bool top = maze[x, y].topWall;
                bool left = maze[x, y].leftWall;
                bool right = maze[x, y].rightWall;
                bool bottom = maze[x, y].bottomWall;

                mazeCell.Init(top, bottom, right, left);
            }
        }

        // Build the NavMesh after the maze is generated
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
