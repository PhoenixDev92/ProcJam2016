using UnityEngine;
using System.Collections;

public class Wall
{
    public string cell1Idx;
    public string cell2Idx;
    public string cell1Label;
    public string cell2Label;

    // Determine if this wall is the same as another
    public bool IsEqual(Wall other)
    {
        if (this.cell1Idx.Equals(other.cell1Idx) && this.cell2Idx.Equals(other.cell2Idx))
        {
            return true;
        }
        if (this.cell1Idx.Equals(other.cell2Idx) && this.cell2Idx.Equals(other.cell1Idx))
        {
            return true;
        }

        return false;
    }
}
