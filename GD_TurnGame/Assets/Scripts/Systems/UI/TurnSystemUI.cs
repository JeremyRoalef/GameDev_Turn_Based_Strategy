using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TurnSystemUI : MonobehaviourEventListener
{
    [SerializeField]
    Button endTurnButton;

    [SerializeField]
    TextMeshProUGUI turnNumberText;

    [SerializeField]
    GameObject enemyTurnVisualObj;

    private void Start()
    {
        SubscribeEvents();
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }



    protected override void SubscribeEvents()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    protected override void UnsubscribeEvents()
    {
        endTurnButton.onClick.RemoveListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
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
