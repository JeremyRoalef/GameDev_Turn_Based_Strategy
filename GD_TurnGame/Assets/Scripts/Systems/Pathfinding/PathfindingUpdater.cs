using System;
using UnityEngine;

public class PathfindingUpdater : MonobehaviourEventListener
{
    private void Start()
    {
        SubscribeEvents();
    }



    protected override void SubscribeEvents()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    protected override void UnsubscribeEvents()
    {
        DestructibleCrate.OnAnyDestroyed -= DestructibleCrate_OnAnyDestroyed;
    }



    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;

        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
