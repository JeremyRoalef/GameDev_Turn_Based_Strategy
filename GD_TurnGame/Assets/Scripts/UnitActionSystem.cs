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

    private void Update()
    {
        if (isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            SetBusy(true);
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition, SetBusy);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetBusy(true);
            selectedUnit.GetSpinAction().Spin(SetBusy);
        }
    }

    bool TryHandleUnitSelection()
    {
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
        OnSelectedUnitChanged?.Invoke(this, selectedUnit);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    void SetBusy(bool newBusyState)
    {
        isBusy = newBusyState;
    }
}
