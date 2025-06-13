using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;
    GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void Damage()
    {
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
