using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class MazeGenerator : MonoBehaviour
{
    [Range(5, 500)]
    public int mazeWidth = 5, mazeHeight = 5; // Dimensions of the maze
    public int starX, starY; // Starting coordinates for maze generation
    MazeCell[,] maze; // 2D array to hold the maze cells

    Vector2Int currentCell; // Current cell being processed

    [SerializeField] GameObject itemPrefab; // Prefab for the item
    [SerializeField] GameObject enemyPrefab; // Enemy prefab

    // Method to generate and return the maze
    public MazeCell[,] GetMaze()
    {
        maze = new MazeCell[mazeWidth, mazeHeight];

        // Initialize the maze cells
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        // Create a random entrance
        CreateRandomEntrance();

        CarvePath(starX, starY); // Start carving the path from the starting coordinates

        // Place the item in the maze
        PlaceItem();



        // Place enemies in the maze
        PlaceEnemies();

        return maze;
    }

    void PlaceEnemies()
    {
        int numberOfEnemies = 5; // Adjust the number of enemies as needed
        float minDistance = 3.0f; // Minimum distance between enemies
        List<Vector2Int> enemyPositions = new List<Vector2Int>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2Int randomPosition;
            bool validPosition;

            do
            {
                validPosition = true;
                randomPosition = new Vector2Int(Random.Range(0, mazeWidth), Random.Range(0, mazeHeight));

                foreach (var pos in enemyPositions)
                {
                    if (Vector2Int.Distance(randomPosition, pos) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            enemyPositions.Add(randomPosition);
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(randomPosition.x, 0, randomPosition.y), Quaternion.identity);
            enemy.GetComponent<EnemyMovement>().Initialize(mazeWidth, mazeHeight);
        }
    }

    // Method to create a random entrance
    void CreateRandomEntrance()
    {
        int entrance = mazeWidth / 2;
        maze[entrance, 0].bottomWall = false;
        Debug.Log($"Entrance created at bottom cell ({entrance}, 0)");
    }

    // Method to place an item in the maze
    void PlaceItem()
    {
        int entranceX = mazeWidth / 2;
        int entranceY = 0;
        float minDistance = mazeHeight * 0.5f;

        List<Vector2Int> validPositions = new List<Vector2Int>();

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                float distance = Vector2Int.Distance(new Vector2Int(x, y), new Vector2Int(entranceX, entranceY));
                if (distance >= minDistance && IsPathCell(maze[x, y]))
                {
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }

        if (validPositions.Count > 0)
        {
            Vector2Int itemPosition = validPositions[Random.Range(0, validPositions.Count)];
            Instantiate(itemPrefab, new Vector3(itemPosition.x, 0, itemPosition.y), Quaternion.identity);
            Debug.Log($"Item placed at cell ({itemPosition.x}, {itemPosition.y})");
        }
        else
        {
            Debug.LogWarning("No valid position found for the item.");
        }
    }

    bool IsPathCell(MazeCell cell)
    {
        return !cell.topWall || !cell.bottomWall || !cell.leftWall || !cell.rightWall;
    }


    // List of possible directions to move
    List<Direction> directions = new List<Direction> {
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    // Method to get a list of random directions
    List<Direction> GetRandomDirections()
    {
        List<Direction> dir = new List<Direction>(directions);
        List<Direction> rndDir = new List<Direction>();

        // Shuffle the directions
        while (dir.Count > 0)
        {
            int rnd = Random.Range(0, dir.Count);
            rndDir.Add(dir[rnd]);
            dir.RemoveAt(rnd);
        }

        return rndDir;
    }

    // Method to check if a cell is valid (within bounds and not visited)
    bool IsCellValid(int x, int y)
    {
        if (x < 0 || y < 0 || x >= mazeWidth || y >= mazeHeight || maze[x, y].visited) return false;
        else return true;
    }

    // Method to check for a valid neighboring cell
    Vector2Int CheckNeighbour()
    {
        List<Direction> rndDir = GetRandomDirections();

        for (int i = 0; i < rndDir.Count; i++)
        {
            Vector2Int neighbour = currentCell;

            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
            }
            if (IsCellValid(neighbour.x, neighbour.y)) return neighbour;
        }
        return currentCell;
    }

    // Method to break walls between two cells
    void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        if (primaryCell.x > secondaryCell.x)
        {
            // primaryCell is to the right of secondaryCell
            maze[primaryCell.x, primaryCell.y].leftWall = false;
            maze[secondaryCell.x, secondaryCell.y].rightWall = false;
        }
        else if (primaryCell.x < secondaryCell.x)
        {
            // primaryCell is to the left of secondaryCell
            maze[primaryCell.x, primaryCell.y].rightWall = false;
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y > secondaryCell.y)
        {
            // primaryCell is above secondaryCell
            maze[primaryCell.x, primaryCell.y].bottomWall = false;
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
        else if (primaryCell.y < secondaryCell.y)
        {
            // primaryCell is below secondaryCell
            maze[primaryCell.x, primaryCell.y].topWall = false;
            maze[secondaryCell.x, secondaryCell.y].bottomWall = false;
        }
    }

    // Method to carve a path in the maze starting from (x, y)
    void CarvePath(int x, int y)
    {
        if (x < 0 || y < 0 || x >= mazeWidth || y >= mazeHeight)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0, 0");
        }

        currentCell = new Vector2Int(x, y);
        maze[currentCell.x, currentCell.y].visited = true; // Mark starting cell as visited

        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(currentCell);

        bool deadEnd = false;
        while (!deadEnd)
        {
            Vector2Int nextCell = CheckNeighbour();

            if (nextCell == currentCell)
            {
                if (path.Count > 0)
                {
                    currentCell = path[path.Count - 1];
                    path.RemoveAt(path.Count - 1);
                }
                else
                {
                    deadEnd = true;
                }
            }
            else
            {
                BreakWalls(currentCell, nextCell);
                maze[nextCell.x, nextCell.y].visited = true; // Mark next cell as visited
                currentCell = nextCell;
                path.Add(currentCell);
            }
        }
    }
}


// Enum to represent possible directions
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// Class to represent a cell in the maze
public class MazeCell
{
    public bool visited; // Whether the cell has been visited
    public int x, y; // Coordinates of the cell

    public bool topWall; // Whether the cell has a top wall
    public bool leftWall; // Whether the cell has a left wall
    public bool rightWall; // Whether the cell has a right wall
    public bool bottomWall; // Whether the cell has a bottom wall

    // Property to get the position of the cell as a Vector2Int
    public Vector2Int position
    {
        get
        {
            return new Vector2Int(x, y);
        }
    }

    // Constructor to initialize the cell
    public MazeCell(int x, int y)
    {
        this.x = x;
        this.y = y;

        visited = false;

        // Initialize all walls to true
        topWall = leftWall = rightWall = bottomWall = true;
    }
}
