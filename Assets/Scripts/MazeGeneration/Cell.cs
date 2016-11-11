using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class Cell
{
    public enum CellState { BLOCKED, PASSAGE};

    public string label;
    public List<Wall> walls;
    public bool visited = false;

    public CellState state;

    public Cell()
    {
        state = CellState.BLOCKED;
    }
}
