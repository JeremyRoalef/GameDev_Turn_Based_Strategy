using UnityEngine;

public abstract class MonobehaviourEventListener : MonoBehaviour
{
    public virtual void OnDisable()
    {
        SubscribeEvents();
    }

    public virtual void OnDestroy()
    {
        UnsubscribeEvents();
    }

    /// <summary>
    /// Must manually set up when subscription occurs
    /// </summary>
    protected abstract void SubscribeEvents();

    /// <summary>
    /// Unsubscribes from events OnDisable and OnDestroy automatically
    /// </summary>
    protected abstract void UnsubscribeEvents();
}
