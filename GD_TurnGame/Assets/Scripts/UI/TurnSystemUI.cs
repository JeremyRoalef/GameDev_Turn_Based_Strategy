using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TurnSystemUI : MonoBehaviour
{
    [SerializeField]
    Button endTurnButton;

    [SerializeField]
    TextMeshProUGUI turnNumberText;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
    }
}
