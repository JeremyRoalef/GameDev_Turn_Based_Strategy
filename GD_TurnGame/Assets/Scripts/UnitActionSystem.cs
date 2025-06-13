using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public event EventHandler<Unit> OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    public static UnitActionSystem Instance { get; private set; }

    [SerializeField]
    Unit selectedUnit;

    [SerializeField]
    LayerMask unitLayerMask;

    BaseAction selectedAction;
    bool isBusy;

    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy) return;
        if (!TurnSystem.Instance.IsPlayerTurn()) return;


        //Mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    bool TryHandleUnitSelection()
    {
        if (!InputManager.Instance.IsMouseButtonDown()) return false;

        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit newUnit))
            {
                if (newUnit == selectedUnit) { return false; }
                if (newUnit.IsEnemy())
                {
                    return false;
                }

                SetSelectedUnit(newUnit);
                return true;
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        OnSelectedUnitChanged?.Invoke(this, selectedUnit);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    void SetBusy(bool newBusyState)
    {
        isBusy = newBusyState;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if (!selectedUnit.TrySpendActionPoints(selectedAction)) return;

            SetBusy(true);
            selectedAction.TakeAction(mouseGridPosition, SetBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
