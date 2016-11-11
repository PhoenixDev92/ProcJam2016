using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class MazeRendering : MonoBehaviour {

    [SerializeField]
    private GameController gameController;

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

    // Field used to construct gameplay within generated maze
    private Vector3 startingPosition;
    private int startingGridRow;
    private int startingGridCol;
    private int destGridRow;
    private int destGridCol;
    private int lowestRow = int.MaxValue;
    private int lowestCol = int.MaxValue;
    private int highestRow = int.MinValue;
    private int highestCol = int.MinValue;

    private int minMovesToDestination = 0;
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

    public int MinMovesToDestination
    {
        get { return minMovesToDestination; }
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

        // Keep track of the upper most and lower most passage cell
        if (type != Tile.TileType.Black)
        {
            lowestRow = (row < lowestRow) ? row : lowestRow;
            lowestCol = (col < lowestCol) ? col : lowestCol;

            highestRow = (row > highestRow) ? row : highestRow;
            highestCol = (col > highestCol) ? col : highestCol;
        }
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
        destGridRow = highestRow;
        destGridCol = highestCol;
        renderGrid[destGridRow, destGridCol].GetComponent<Tile>().Type = Tile.TileType.Red;

        Pathfinding pathFinder = new Pathfinding();
        minMovesToDestination = pathFinder.FindShortestPath(renderGrid, renderRows, renderColumns, startingGridRow, startingGridCol, highestRow, highestCol);
        Debug.Log(minMovesToDestination);

        // Set the flag to indicate that maze generation is complete
        isMazeGenerated = true;
    }


    // Calculates a position based on the row and col values
    public Vector3 CalculatePlayerGridPosition(int desiredRow, int desiredCol, int currentRow, int currentCol, Vector3 currentPosition)
    {
        if (Mathf.Abs(desiredRow - currentRow) > 1 || Mathf.Abs(desiredCol - currentCol) > 1)
        {
            return currentPosition;
        }

        desiredRow = Mathf.Clamp(desiredRow, 0, renderRows - 1);
        desiredCol = Mathf.Clamp(desiredCol, 0, renderColumns - 1);

        if (renderGrid[desiredRow, desiredCol].GetComponent<Tile>().Type != Tile.TileType.Black)
        {
            // Check if the player has reached the destination
            if (desiredRow == destGridRow && desiredCol == destGridCol)
            {
                gameController.PlayerReachedDestination();
            }

            float xPos = startingX + (desiredCol * colIncrement);
            float yPos = startingY - (desiredRow * rowIncrement);

            return new Vector3(xPos, yPos, currentPosition.z);
        }

        return currentPosition;
    }

    // Verify that the specified cell is walkable and within the grid
    public bool VerifyCell (int row, int col)
    {
        if (row >= 0 && row < renderRows)
        {
            if (col >= 0 && col < renderColumns)
            {
                return renderGrid[row, col].GetComponent<Tile>().Type != Tile.TileType.Black;
            }
        }

        return false;
    }

    // Resets certain values associated with the maze and removes the current one
    public void Reset()
    {
        // Reset values
        lowestRow = int.MaxValue;
        lowestCol = int.MaxValue;
        highestRow = int.MinValue;
        highestCol = int.MinValue;
        isMazeGenerated = false;

        // Destroy existing GameObjects
        for (int r = 0; r < renderRows; r++)
        {
            for (int c = 0; c < renderColumns; c++)
            {
                GameController.Destroy(renderGrid[r, c]);
            }
        }
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

    public class CalculatedPlayerPositionData
    {
        public Vector3 position;
        public int row;
        public int col;

        public CalculatedPlayerPositionData(Vector3 pos, int r, int c)
        {
            position = pos;
            row = r;
            col = c;
        }
    }
}
