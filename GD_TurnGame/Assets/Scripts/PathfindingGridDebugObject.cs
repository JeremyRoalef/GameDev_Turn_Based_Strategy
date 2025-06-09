using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField]
    TextMeshPro gCostText;

    [SerializeField]
    TextMeshPro hCostText;

    [SerializeField]
    TextMeshPro fCostText;

    PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
    }
}
