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

    [SerializeField]
    GameObject enemyTurnVisualObj;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    void UpdateTurnText()
    {
        turnNumberText.text = $"Turn {TurnSystem.Instance.GetTurnNumber()}";
    }

    void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualObj.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
