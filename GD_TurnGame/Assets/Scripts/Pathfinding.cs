using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    Transform gridDebugObjectPrefab;

    public static Pathfinding Instance { get; private set; }

    int width;
    int height;
    int cellSize;
    GridSystem<PathNode> gridSystem;

    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gridSystem = new GridSystem<PathNode>(
            10,
            10,
            2f,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition)
            );
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        //Nodes queued for searching
        List<PathNode> openList = new List<PathNode> ();

        //Nodes already searched
        List<PathNode> closedList = new List<PathNode>();

        //Starting node to pathfind from
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        //Reset pathfinding system
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        //Initialize start node
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        //Search for path
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            //Check if end node reached
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            //Handle list logic
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //calculate G, H, and F Cost for all neighbor nodes
            foreach (PathNode neighborNode in GetNeighborList(currentNode))
            {
                if (closedList.Contains(neighborNode)) { continue; }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(
                    currentNode.getGridPosition(), 
                    neighborNode.getGridPosition()
                    );

                //Check if this pathfinding is better than previous
                if (tentativeGCost < neighborNode.GetGCost())
                {
                    //Better path found. Recalculate costs
                    neighborNode.SetCameFromPathNode(currentNode);
                    neighborNode.SetGCost(tentativeGCost);
                    neighborNode.SetHCost(CalculateDistance(neighborNode.getGridPosition(), endGridPosition));
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        //No path found
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCost = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCost.GetFCost())
            {
                lowestFCost = pathNodeList[i];
            }
        }
        return lowestFCost;
    }

    List<PathNode> GetNeighborList(PathNode currentNode)
    {
        //List to return
        List<PathNode> neighborList = new List<PathNode>();

        //Testing grid position
        GridPosition gridPosition = currentNode.getGridPosition();

        //Check all left nodes
        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Left Up
                neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                //Left Down
                neighborList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
        }

        //Check all right nodes
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //Right Up
                neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }

            if (gridPosition.z - 1 >= 0)
            {
                //Right Down
                neighborList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
        }

        //Check for up node
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighborList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        //Check for down node
        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighborList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        //Return list
        return neighborList;
    }

    PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    List<GridPosition> CalculatePath(PathNode endNode)
    {
        //List of path nodes
        List<PathNode> pathNodeList = new List<PathNode>();

        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;

        //Add connecting nodes
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        //Reverse order from start to end
        pathNodeList.Reverse();

        //Convert into grid positions
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach(PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.getGridPosition());
        }

        //Return grid positions
        return gridPositionList;
    }
}
