using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshPro;

    [SerializeField]
    Button button;

    public void SetBaseAction(BaseAction baseAction)
    {
        textMeshPro.text = baseAction.GetActionName().ToUpper();
    }
}
