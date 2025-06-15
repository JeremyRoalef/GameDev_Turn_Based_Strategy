using System;
using UnityEngine;

public interface IInteractible
{
    void Interact(Action onInteractionComplete);
}
