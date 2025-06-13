using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    int maxINteractDistance = 1;

    private void Update()
    {
        if (!isActive) return;
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxINteractDistance; x <= maxINteractDistance; x++)
        {
            for (int z = -maxINteractDistance; z <= maxINteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                Vector3 unitWorldPos = LevelGrid.Instance.GetWorldPosition(unitGridPosition);

                //Conditions to continue
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete)
    {
        Debug.Log("Interact");
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractCompelte);
        StartAction(OnActionComplete);
    }

    void OnInteractCompelte()
    {
        CompleteAction();
    }
}
