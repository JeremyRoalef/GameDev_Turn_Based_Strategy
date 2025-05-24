using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField]
    Animator unitAnimator;

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
            unitAnimator.SetBool("IsWalking", true);

            transform.position += moveDir * Time.deltaTime * moveSpeed;

        }
        else
        {
            //Snap to position
            unitAnimator.SetBool("IsWalking", false);
            transform.position = targetPos;
            isActive = false;
            OnActionComplete?.Invoke(false);
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    public override void TakeAction(GridPosition targetPos, Action<bool> OnActionComplete)
    {
        base.OnActionComplete = OnActionComplete;
        unitAnimator.SetBool("IsWalking", true);
        this.targetPos = LevelGrid.Instance.GetWorldPosition(targetPos);
        isActive = true;
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
}
