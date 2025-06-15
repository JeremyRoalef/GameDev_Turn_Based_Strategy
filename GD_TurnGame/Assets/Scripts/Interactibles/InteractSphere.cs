using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractible
{
    public enum InteractSphereState
    {
        Green,
        Red
    }

    [Header("References")]
    [SerializeField]
    Material greenMaterial;

    [SerializeField]
    Material redMaterial;

    [SerializeField]
    MeshRenderer meshRenderer;

    [SerializeField]
    InteractSphereState interactSphereState;

    [Header("Settings")]
    [SerializeField]
    float interactionDuration = 0.5f;

    Action onInteractionComplete;
    GridPosition gridPosition;

    bool isActive = false;


    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractibleAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive) return;

        interactionDuration -= Time.deltaTime;

        if (interactionDuration <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    void SetColorGreen()
    {
        interactSphereState = InteractSphereState.Green;
        meshRenderer.material = greenMaterial;
    }

    void SetColorRed()
    {
        interactSphereState = InteractSphereState.Red;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        isActive = true;
        this.onInteractionComplete = onInteractionComplete;

        switch (interactSphereState)
        {
            case InteractSphereState.Green:
                SetColorRed();
                break;
            case InteractSphereState.Red:
                SetColorGreen();
                break;
            default:
                SetColorGreen();
                break;
        }
    }
}
