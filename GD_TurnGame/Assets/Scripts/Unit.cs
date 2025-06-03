using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    GridPosition gridPosition;
    MoveAction moveAction;
    SpinAction spinAction;
    BaseAction[] baseActionArray;
    HealthSystem healthSystem;


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
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
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
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, newGridPosition);
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

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
