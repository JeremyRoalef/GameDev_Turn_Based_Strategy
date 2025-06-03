using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    [SerializeField]
    float spinSpeed = 360f;

    float spinAmount = 0;
    bool isSpinning;

    private void Update()
    {
        if (!isActive) return;

        if (isSpinning)
        {
            spinAmount += spinSpeed * Time.deltaTime;

            transform.eulerAngles += new Vector3(
                0,
                spinSpeed * Time.deltaTime,
                0
                );

            if (spinAmount >= 360f)
            {
                isSpinning = false;
                CompleteAction();
                spinAmount = 0;
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete)
    {
        isSpinning = true;
        StartAction(OnActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
