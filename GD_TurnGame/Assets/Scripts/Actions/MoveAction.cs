using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField]
    float moveSpeed = 4f;

    [SerializeField]
    float rotateSpeed = 10f;

    [SerializeField]
    int maxMoveDistance = 4;

    Vector3 targetPos;
    const float nearTargetPosDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();

        targetPos = transform.position;
    }

    public MoveAction GetMoveAction()
    {
        return GetComponent<MoveAction>();
    }

    private void Update()
    {
        if (!isActive) return;

        Vector3 moveDir = (targetPos - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPos) > nearTargetPosDistance)
        {
            //Smooth move to position
            transform.position += moveDir * Time.deltaTime * moveSpeed;

        }
        else
        {
            //Snap to position
            transform.position = targetPos;
            CompleteAction();
            OnStopMoving(this, EventArgs.Empty);
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public override void TakeAction(GridPosition targetPos, Action<bool> OnActionComplete)
    {
        this.targetPos = LevelGrid.Instance.GetWorldPosition(targetPos);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        StartAction(OnActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                //Conditions to continue
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testGridPosition == unitGridPosition) continue;
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCount = unit.GetShootAction().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCount * 10
        };
    }
}
