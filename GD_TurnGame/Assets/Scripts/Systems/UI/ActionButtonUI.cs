using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonobehaviourEventListener
{
    [SerializeField]
    TextMeshProUGUI textMeshPro;

    [SerializeField]
    Button button;

    [SerializeField]
    GameObject selectedGameObject;

    BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        SubscribeEvents();
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedBaseAction == baseAction)
        {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }

    protected override void SubscribeEvents()
    {
        //This setup is the same as doing a simple add listener for the UnityEvent
        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    protected override void UnsubscribeEvents()
    {
        //This setup is the same as doing a simple add listener for the UnityEvent
        button.onClick.RemoveListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
}
