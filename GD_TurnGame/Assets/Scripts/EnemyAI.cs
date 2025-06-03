using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    State state;

    float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }


    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    state = State.Busy;
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        //No more enemy actions available
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:

                break;
        }
    }

    void SetStateTakingTurn(bool isTakingTurn)
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    bool TryTakeEnemyAIAction(Action<bool> onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            Debug.Log("Taking enemy AI Action");
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    bool TryTakeEnemyAIAction(Unit enemyUnit, Action<bool> onEnemyAIActionComplete)
    {

        Debug.Log("Taking spin action");
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) return false;
        if (!enemyUnit.TrySpendActionPoints(spinAction)) return false;

        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }
}
