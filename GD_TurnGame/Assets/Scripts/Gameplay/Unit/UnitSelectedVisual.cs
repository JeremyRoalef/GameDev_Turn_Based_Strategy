using System;
using UnityEngine;

public class UnitSelectedVisual : MonobehaviourEventListener
{
    [SerializeField]
    Unit unit;

    [SerializeField]
    MeshRenderer meshRenderer;

    private void Start()
    {
        SubscribeEvents();
        UpdateVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }



    protected override void SubscribeEvents()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    protected override void UnsubscribeEvents()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
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
}
