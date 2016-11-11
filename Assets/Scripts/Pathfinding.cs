using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Priority_Queue;

public class Pathfinding {

    public GridCell[,] maze;
    public SimplePriorityQueue <GridCell> openList;
    public List<GridCell> closedList;
    public int startRow, startCol, destRow, destCol;
    public int totalRows, totalCols;

    public int FindShortestPath(GameObject[,] generatedMaze, int numRows, int numCols, int startR, int startC, int destR, int destC)
    {
        startRow = startR;
        startCol = startC;
        destRow = destR;
        destCol = destC;

        totalRows = numRows;
        totalCols = numCols;

        InitPathfindingMaze(generatedMaze, numRows, numCols);
        return FindPath();
    }

    private void InitPathfindingMaze(GameObject[,] generatedMaze, int numRows, int numCols)
    {
        maze = new GridCell[numRows, numCols];

        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                maze[r, c] = new GridCell();
                maze[r, c].row = r;
                maze[r, c].col = c;
                maze[r, c].walkable = (generatedMaze[r, c].GetComponent<Tile>().Type != Tile.TileType.Black);
            }
        }
    }

    private int FindPath()
    {
        openList = new SimplePriorityQueue<GridCell>();
        closedList = new List<GridCell>();

        // Add the starting cell to the open list
        GridCell startingCell = maze[startRow, startCol];
        openList.Enqueue(startingCell, startingCell.f);

        while (openList.Count != 0)
        {
            GridCell currentSquare = openList.Dequeue();
            closedList.Add(currentSquare);

            if (currentSquare.row == destRow && currentSquare.col == destCol)
            {
                return currentSquare.g;
            }

            // Check the four possible neighbour squares
            GridCell top = (currentSquare.row != 0) ? maze[currentSquare.row - 1, currentSquare.col] : null;
            GridCell bottom = (currentSquare.row != totalRows - 1) ? maze[currentSquare.row + 1, currentSquare.col] : null;
            GridCell left = (currentSquare.col != 0) ? maze[currentSquare.row, currentSquare.col - 1] : null;
            GridCell right = (currentSquare.col != totalCols - 1) ? maze[currentSquare.row, currentSquare.col + 1] : null;

            // Process each of the potential neighbour cells
            ProcessCell(currentSquare, top, openList, closedList);
            ProcessCell(currentSquare, bottom, openList, closedList);
            ProcessCell(currentSquare, left, openList, closedList);
            ProcessCell(currentSquare, right, openList, closedList);
        }

        return -1;
    }

    // Determine what to with the given cell in terms of adding it to the open list
    private void ProcessCell (GridCell currentSquare, GridCell cell, SimplePriorityQueue<GridCell> openList, List<GridCell> closedList)
    {
        if (cell != null)
        {
            if (cell.walkable && !closedList.Contains(cell))
            {
                if (!openList.Contains(cell))
                {
                    cell.g = currentSquare.g + 1;
                    cell.f = cell.g + cell.CalculateH(destRow, destCol);
                    openList.Enqueue(cell, cell.f);
                }
                else
                {
                    // If this path would be shorter
                    if (cell.g > (currentSquare.g + 1))
                    {
                        cell.g = currentSquare.g + 1;
                        cell.f = cell.g + cell.CalculateH(destRow, destCol);
                        openList.UpdatePriority(cell, cell.f);
                    }
                }
            }
        }
    }

    public class GridCell : IEqualityComparer<GridCell>
    {
        public int row, col;
        public bool walkable;
        public int f = 0;
        public int g = 0;
        public int h = 0;

        // Use the manhattan mehtod to calculate the h value
        public int CalculateH(int destRow, int destCol)
        {
            return (10 * (Mathf.Abs(row - destRow) + (Mathf.Abs(col - destCol))));
        }

        public bool Equals(GridCell x, GridCell y)
        {
            return (x.row == y.row && x.col == y.col);
        }

        public int GetHashCode(GridCell obj)
        {
            return f;
        }
    }
}
