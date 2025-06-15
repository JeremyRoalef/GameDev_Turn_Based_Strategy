using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;

    [Header("References")]
    [SerializeField]
    LayerMask obstaclesLayermask;

    [Header("Settings")]
    [SerializeField]
    int damageAmount = 35;

    [SerializeField]
    float aimingStateTime = 0.5f;

    [SerializeField]
    float shootingStateTime = 0.5f;

    [SerializeField]
    float cooloffStateTime = 0.5f;

    [SerializeField]
    float rotateSpeed = 10f;

    enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    State state;
    Vector3 targetPos;
    Unit targetUnit;

    float stateTimer;
    int maxShootDistance = 7;
    bool canShootBullet;

    private void Update()
    {
        if (!isActive) return;

        HandleSwitchStates();
    }

    private void HandleSwitchStates()
    {
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                AimAtTarget();
                if (stateTimer <= 0)
                {
                    state = State.Shooting;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Shooting:
                if (stateTimer <= 0)
                {
                    state = State.Cooloff;
                    stateTimer = cooloffStateTime;
                }
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    CompleteAction();
                }
                break;
        }
    }

    private void AimAtTarget()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        targetUnit.Damage(damageAmount);
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                float unitShoulderHeight = 1.7f;
                Vector3 unitWorldPos = LevelGrid.Instance.GetWorldPosition(unitGridPosition);

                //Conditions to continue
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testDistance > maxShootDistance) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                Debug.Log(targetUnit);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPos).normalized;

                if (Physics.Raycast(
                    unitWorldPos + Vector3.up * unitShoulderHeight,
                    shootDir,
                    Vector3.Distance(unitWorldPos, targetUnit.GetWorldPosition()),
                    obstaclesLayermask
                    ))
                {
                    //Blocked by an obstacle
                    continue;
                }

                //If on same team, continue
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete)
    {
        state = State.Aiming;
        stateTimer = aimingStateTime;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        this.targetPos = LevelGrid.Instance.GetWorldPosition(gridPosition);

        StartAction(OnActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
