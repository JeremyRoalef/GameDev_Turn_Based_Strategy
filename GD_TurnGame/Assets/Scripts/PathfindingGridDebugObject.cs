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

    [SerializeField]
    SpriteRenderer spriteRenderer;

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
        spriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
