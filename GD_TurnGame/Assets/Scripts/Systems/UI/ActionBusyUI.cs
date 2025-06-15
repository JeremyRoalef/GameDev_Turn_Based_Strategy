using System;
using UnityEngine;

public class ActionBusyUI : MonobehaviourEventListener
{
    private void Start()
    {
        SubscribeEvents();
        gameObject.SetActive(false);
    }



    protected override void SubscribeEvents()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    protected override void UnsubscribeEvents()
    {
        UnitActionSystem.Instance.OnBusyChanged -= UnitActionSystem_OnBusyChanged;
    }



    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
