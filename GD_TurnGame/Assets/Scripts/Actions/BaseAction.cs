using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action<bool> OnActionComplete;
    protected Unit unit;
    protected bool isActive;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        //Default cost
        return 1;
    }

    protected void StartAction(Action<bool> OnActionComplete)
    {
        isActive = true;
        this.OnActionComplete = OnActionComplete;
    }

    protected void CompleteAction()
    {
        isActive = false;
        OnActionComplete?.Invoke(false);
    }
}
