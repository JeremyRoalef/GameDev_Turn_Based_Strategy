using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonobehaviourEventListener
{
    [SerializeField]
    Unit unit;

    [SerializeField]
    HealthSystem healthSystem;

    [SerializeField]
    TextMeshProUGUI actionPointsText;

    [SerializeField]
    Image healthBarImage;

    private void Start()
    {
        UpdateActionPointsText();
        UpdateHealthBar();
        SubscribeEvents();
    }



    protected override void SubscribeEvents()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    protected override void UnsubscribeEvents()
    {
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
    }



    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }



    void UpdateActionPointsText()
    {
        if (unit == null) return;
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
