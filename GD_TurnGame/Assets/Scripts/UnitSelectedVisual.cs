using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField]
    Unit unit;

    [SerializeField]
    MeshRenderer meshRenderer;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit selectedUnit)
    {
        UpdateVisual(selectedUnit);
    }

    private void UpdateVisual(Unit selectedUnit)
    {
        if (unit == selectedUnit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }
}
