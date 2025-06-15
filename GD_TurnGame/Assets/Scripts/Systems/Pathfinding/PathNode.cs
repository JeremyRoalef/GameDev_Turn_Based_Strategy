using System;
using UnityEngine;

public class PathNode
{
    GridPosition gridPosition;
    PathNode cameFromPathNode;

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

    bool isWalkable = true;

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

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }

    public GridPosition getGridPosition()
    {
        return gridPosition;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        cameFromPathNode = pathNode;
    }

    public PathNode GetCameFromPathNode()
    {
        return cameFromPathNode;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
}
