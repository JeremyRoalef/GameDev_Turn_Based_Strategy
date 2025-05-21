using System;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField]
    Transform actionButtonPrefab;

    [SerializeField]
    Transform actionButtonContainerTransform;

    private void Start()
    {
        CreateUnitActionButtons();
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit e)
    {
        CreateUnitActionButtons();
    }

    private void Update()
    {
        
    }

    void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButton = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }
}
