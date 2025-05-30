using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;

    GridPosition gridPosition;
    MoveAction moveAction;
    SpinAction spinAction;
    BaseAction[] baseActionArray;

    [SerializeField]
    int startingActionPoints = 2;

    [SerializeField]
    bool isEnemy;

    int actionPoints;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        actionPoints = startingActionPoints;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        //Refresh action points for enemy & player logic
        if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!isEnemy && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = startingActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPoints(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        if (actionPoints >= action.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SpendActionPoints(int amount)
    {
        actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage()
    {
        Debug.Log($"{gameObject.name} damaged");
    }
    
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
