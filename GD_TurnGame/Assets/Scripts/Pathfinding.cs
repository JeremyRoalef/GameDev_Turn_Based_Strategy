using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    Transform gridDebugObjectPrefab;

    int width;
    int height;
    int cellSize;
    GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        gridSystem = new GridSystem<PathNode>(
            10,
            10,
            2f,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition)
            );
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
}
