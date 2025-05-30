using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI actionPointsText;

    [SerializeField]
    Unit unit;

    [SerializeField]
    Image healthBarImage;

    [SerializeField]
    HealthSystem healthSystem;

    private void Start()
    {
        UpdateActionPointsText();
        UpdateHealthBar();
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
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
