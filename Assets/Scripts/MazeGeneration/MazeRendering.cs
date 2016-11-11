using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class MazeRendering : MonoBehaviour {

    [Header("Tile Settings")]
    [SerializeField]
    private GameObject tile;

    [Header("Maze Area Settings")]
    [SerializeField]
    private float boundsX;
    [SerializeField]
    private float boundsY;

    private int renderRows;
    private int renderColumns;
    private GameObject[,] renderGrid;

    private float startingX;
    private float startingY;
    private float rowIncrement;
    private float colIncrement;
    private float objectBaseSize = 14.07975f;
    private Vector3 objectCalculatedScale;

    // Fields use by the GameController to spawn the player
    private Vector3 startingPosition;
    private int startingGridRow;
    private int startingGridCol;
    private int lowestRow = int.MaxValue;
    private int lowestCol = int.MaxValue;
    private int highestRow = int.MinValue;
    private int highestCol = int.MinValue;

    private bool isMazeGenerated = false;

    public Vector3 ObjectCalculatedScale
    {
        get { return objectCalculatedScale; }
    }

    public float RowIncrement
    {
        get { return rowIncrement; }
    }

    public float ColIncrement
    {
        get { return colIncrement; }
    }

    public Vector3 StartingPosition { get { return startingPosition; } }
    public int StartingGridRow { get { return startingGridRow; } }
    public int StartingGridCol { get { return startingGridCol; } }
    public bool IsMazeGenerated { get { return isMazeGenerated; } }

    // Places the tiles on the grid
    public void placeTiles(int generationRows, int generationCols)
    {
        renderRows = generationRows;
        renderColumns = generationCols;
        renderGrid = new GameObject[renderRows, renderColumns];

        startingX = -boundsX / 2f;
        startingY = boundsY / 2f;

        rowIncrement = boundsY / renderRows;
        colIncrement = boundsX / renderColumns;
        objectCalculatedScale = new Vector3(objectBaseSize / renderColumns, objectBaseSize / renderRows, 1);

        for (int r = 0; r < renderRows; r++)
        {
            float currentYPosition = startingY - (r * rowIncrement);

            for (int c = 0; c < renderColumns; c++)
            {
                float currentXPosition = startingX + (c * colIncrement);

                GameObject tempTile = (GameObject)Instantiate(tile, new Vector3(currentXPosition, currentYPosition, 0), Quaternion.identity);
                tempTile.transform.parent = this.transform;
                tempTile.transform.localScale = objectCalculatedScale;
                renderGrid[r, c] = tempTile;
            }
        }
    }

    public void UpdateTile (int row, int col, Tile.TileType type)
    {
        renderGrid[row, col].GetComponent<Tile>().Type = type;

        // Keep track of the upper most passage cell
        if (type != Tile.TileType.Black)
        {
            lowestRow = (row < lowestRow) ? row : lowestRow;
            lowestCol = (col < lowestCol) ? col : lowestCol;
        }

        // Keep track of the lower most passage cell
        if (type != Tile.TileType.Black)
        {
            highestRow = (row > highestRow) ? row : highestRow;
            highestCol = (col > highestCol) ? col : highestCol;
        }
    }

    // Calculates a position based on the row and col values
    public Vector3 CalculatePlayerGridPosition(int row, int col, Vector3 currentPosition)
    {
        row = Mathf.Clamp(row, 0, renderRows - 1);
        col = Mathf.Clamp(col, 0, renderColumns - 1);

        if (renderGrid[row, col].GetComponent<Tile>().Type != Tile.TileType.Black)
        {
            float xPos = startingX + (col * colIncrement);
            float yPos = startingY - (row * rowIncrement);

            return new Vector3(xPos, yPos, currentPosition.z);
        }

        return currentPosition;
    }

    // Used to indicate the end of maze generation
    public void MazeGenerationComplete()
    {
        // Set the start tile
        renderGrid[lowestRow, lowestCol].GetComponent<Tile>().Type = Tile.TileType.Green;

        // Set the player start position
        startingPosition = renderGrid[lowestRow, lowestCol].transform.position;
        startingGridRow = lowestRow;
        startingGridCol = lowestCol;

        // Set the destination tile
        renderGrid[highestRow, highestCol].GetComponent<Tile>().Type = Tile.TileType.Red;

        Pathfinding pathFinder = new Pathfinding();
        int shortestPath = pathFinder.FindShortestPath(renderGrid, renderRows, renderColumns, startingGridRow, startingGridCol, highestRow, highestCol);
        Debug.Log(shortestPath);

        // Set the flag to indicate that maze generation is complete
        isMazeGenerated = true;
    }

    // Draws the generated maze
    /*public void DrawMaze(Cell[,] grid)
    {
        for (int r = 0; r < renderRows; r++)
        {
            for (int c = 0; c < renderColumns; c++)
            {
                if (grid[r, c].state == Cell.CellState.PASSAGE)
                {
                    renderGrid[r, c].GetComponent<Tile>().Type = Tile.TileType.White;
                }
            }
        }
    }*/

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundsX, boundsY, 1));
    }
}
