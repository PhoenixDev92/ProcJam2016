using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class MazeGeneration2 : MonoBehaviour {

    [Header("Maze Generation Size")]
    [SerializeField]
    private int rows;
    [SerializeField]
    private int columns;

    private Cell[,] grid;
    private MazeRendering mazeRenderer;

    void Awake()
    {
        mazeRenderer = GetComponent<MazeRendering>();
    }

    // Use this for initialization
    void Start() {
        mazeRenderer.placeTiles(rows, columns);

        // Initializes the grid
        grid = new Cell[rows, columns];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                grid[r, c] = new Cell();
            }
        }

        StartCoroutine(MazeGenerationCoroutine());
    }

    // Returns the frontier cells at distance 2 and at the desired state of the given cell 
    public List<IndexPair> GetFrontierCellAtDistance2(int row, int column, Cell.CellState desiredState)
    {
        List<IndexPair> frontierCells = new List<IndexPair>();

        // Determine which frontier cells are whithin the grid
        Cell topCell = (row - 2) >= 0 ? grid[row - 2, column] : null;
        Cell bottomCell = (row + 2) < rows ? grid[row + 2, column] : null;
        Cell leftCell = (column - 2) >= 0 ? grid[row, column - 2] : null;
        Cell rightCell = (column + 2) < columns ? grid[row, column + 2] : null;

        // Check the state of the frontier cells whithin the grid
        if (topCell != null && topCell.state == desiredState)
        {
            IndexPair newPair = new IndexPair(row - 2, column);
            frontierCells.Add(newPair);
        }
        if (bottomCell != null && bottomCell.state == desiredState)
        {
            IndexPair newPair = new IndexPair(row + 2, column);
            frontierCells.Add(newPair);
        }
        if (leftCell != null && leftCell.state == desiredState)
        {
            IndexPair newPair = new IndexPair(row, column - 2);
            frontierCells.Add(newPair);
        }
        if (rightCell != null && rightCell.state == desiredState)
        {
            IndexPair newPair = new IndexPair(row, column + 2);
            frontierCells.Add(newPair);
        }

        return frontierCells;
    }

    // Given two index pairs, finds the index of the cell between them
    public IndexPair FindInBetweenCell(IndexPair frontier, IndexPair neighbour)
    {
        // Cells are on separate rows
        if (frontier.rowIndex != neighbour.rowIndex)
        {
            if (frontier.rowIndex > neighbour.rowIndex)
            {
                return new IndexPair(frontier.rowIndex - 1, frontier.columnIndex);
            }
            else
            {
                return new IndexPair(frontier.rowIndex + 1, frontier.columnIndex);
            }
        }
        if (frontier.columnIndex != neighbour.columnIndex)
        {
            if (frontier.columnIndex > neighbour.columnIndex)
            {
                return new IndexPair(frontier.rowIndex, frontier.columnIndex - 1);
            }
            else
            {
                return new IndexPair(frontier.rowIndex, frontier.columnIndex + 1);
            }
        }

        return null;
    }

    public IEnumerator MazeGenerationCoroutine()
    {
        int randomRow = Random.Range(0, rows);
        int randomColumn = Random.Range(0, columns);

        grid[randomRow, randomColumn].state = Cell.CellState.PASSAGE;
        mazeRenderer.UpdateTile(randomRow, randomColumn, Tile.TileType.White);
        List<IndexPair> frontierCells = GetFrontierCellAtDistance2(randomRow, randomColumn, Cell.CellState.BLOCKED);

        while (frontierCells.Count != 0)
        {
            int randomIdx = Random.Range(0, frontierCells.Count);
            IndexPair selectedFrontierCell = frontierCells[randomIdx];

            // Avoids processing the same cell twice
            if (grid[selectedFrontierCell.rowIndex, selectedFrontierCell.columnIndex].state == Cell.CellState.PASSAGE)
            {
                frontierCells.RemoveAt(randomIdx);
                continue;
            }

            grid[selectedFrontierCell.rowIndex, selectedFrontierCell.columnIndex].state = Cell.CellState.PASSAGE; // Marks the selected cell as a passage
            mazeRenderer.UpdateTile(selectedFrontierCell.rowIndex, selectedFrontierCell.columnIndex, Tile.TileType.White);

            List<IndexPair> neighbours = GetFrontierCellAtDistance2(selectedFrontierCell.rowIndex, selectedFrontierCell.columnIndex, Cell.CellState.PASSAGE);
            IndexPair randomNeighbour = neighbours[Random.Range(0, neighbours.Count)];
            IndexPair inbetweenCell = FindInBetweenCell(selectedFrontierCell, randomNeighbour);
            grid[inbetweenCell.rowIndex, inbetweenCell.columnIndex].state = Cell.CellState.PASSAGE;
            mazeRenderer.UpdateTile(inbetweenCell.rowIndex, inbetweenCell.columnIndex, Tile.TileType.White);

            frontierCells.RemoveAt(randomIdx);
            frontierCells.AddRange(GetFrontierCellAtDistance2(selectedFrontierCell.rowIndex, selectedFrontierCell.columnIndex, Cell.CellState.BLOCKED));

            yield return new WaitForSeconds(0.001f);
        }

        mazeRenderer.MazeGenerationComplete();
        yield return null;
    }

    // Stores the index of a cell in a 2D array in a single class
    public class IndexPair
    {
        public int rowIndex;
        public int columnIndex;

        public IndexPair(int row, int column)
        {
            rowIndex = row;
            columnIndex = column;
        }

        // Determines if this index pair is equal to another
        public bool isEqual(IndexPair other)
        {
            return ((this.rowIndex == other.rowIndex) && (this.columnIndex == other.columnIndex));
        }
    }
}
