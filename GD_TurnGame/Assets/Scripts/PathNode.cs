using System;
using UnityEngine;

public class PathNode
{
    GridPosition gridPosition;
    /// <summary>
    /// Cost of walking to the node
    /// </summary>
    int gCost;
    /// <summary>
    /// Heuristic cost of reaching the end node
    /// </summary>
    int hCost;
    /// <summary>
    /// G + H
    /// </summary>
    int fCost;
    PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
}
