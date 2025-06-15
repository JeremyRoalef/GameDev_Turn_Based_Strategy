using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAction : BaseAction
{
    public static event EventHandler OnAnyMeleeWeaponHit;

    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;

    [SerializeField]
    float SwiningSwordBeforeHitTimer = 0.5f;

    [SerializeField]
    float SwingingSwordAfterHitTimer = 0.5f;

    [SerializeField]
    int damageAmount = 150;

    [SerializeField]
    float rotateSpeed = 10f;

    enum State
    {
        SwiningSwordBeforeHit,
        SwingingSwordAfterHit
    }

    State state;
    Vector3 targetPos;
    Unit targetUnit;

    float stateTimer;
    int maxMeleeDistance = 1;

    private void Update()
    {
        if (!isActive) return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwiningSwordBeforeHit:
                AimAtTarget();
                if (stateTimer <= 0)
                {
                    Debug.Log("Hit target");
                    OnAnyMeleeWeaponHit?.Invoke(this, EventArgs.Empty);
                    targetUnit.Damage(damageAmount);
                    state = State.SwingingSwordAfterHit;
                    stateTimer = SwingingSwordAfterHitTimer;
                }
                break;
            case State.SwingingSwordAfterHit:
                if (stateTimer <= 0)
                {
                    Debug.Log("Finished swing");
                    OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
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

    public override string GetActionName()
    {
        return "Melee";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 250
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxMeleeDistance; x <= maxMeleeDistance; x++)
        {
            for (int z = -maxMeleeDistance; z <= maxMeleeDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                Vector3 unitWorldPos = LevelGrid.Instance.GetWorldPosition(unitGridPosition);

                //Conditions to continue
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
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
        Debug.Log("Swinging before hit");
        state = State.SwiningSwordBeforeHit;
        stateTimer = SwiningSwordBeforeHitTimer;

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        this.targetPos = LevelGrid.Instance.GetWorldPosition(gridPosition);

        OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);
        StartAction(OnActionComplete);
    }

    public int GetMaxMeleeDistance()
    {
        return 1;
    }
}
