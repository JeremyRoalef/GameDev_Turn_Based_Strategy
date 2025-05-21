using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action<bool> OnActionComplete;
    protected Unit unit;
    protected bool isActive;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
}
