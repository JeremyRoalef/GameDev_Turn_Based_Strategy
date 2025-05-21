using System;
using UnityEngine;
using UnityEngine.Events;

public class UnitActionSystem : MonoBehaviour
{
    public event EventHandler<Unit> OnSelectedUnitChanged;
    
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
        if (TryHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    bool TryHandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit newUnit))
            {
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
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, selectedUnit);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    void SetBusy(bool newBusyState)
    {
        isBusy = newBusyState;
    }

    void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());

            switch (selectedAction)
            {
                case MoveAction moveAction:
                    SetBusy(true);
                    if (moveAction.IsValidActionGridPosition(mouseGridPosition))
                    {
                        moveAction.Move(mouseGridPosition, SetBusy);
                    }
                    break;
                case SpinAction spinAction:
                    SetBusy(true);
                    spinAction.Spin(SetBusy);
                    break;
            }
        }
    }
}
