using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

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
    Unit targetUnit;
    Vector3 targetPos;

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
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                if (stateTimer <= 0)
                {
                    state = State.Cooloff;
                    stateTimer = cooloffStateTime;
                }
                break;
            case State.Cooloff:
                if (stateTimer <= 0)
                {
                    CompleteAction();
                    OnShoot?.Invoke(this, new OnShootEventArgs
                    {
                        targetUnit = targetUnit,
                        shootingUnit = unit
                    });
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
        targetUnit.Damage();
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);


                //Conditions to continue
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (testDistance > maxShootDistance) continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                //If on same team, continue
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action<bool> OnActionComplete)
    {
        StartAction(OnActionComplete);
        state = State.Aiming;
        stateTimer = aimingStateTime;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        canShootBullet = true;
        this.targetPos = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }
}
