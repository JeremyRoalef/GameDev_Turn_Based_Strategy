using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractible
{
    [SerializeField]
    Material greenMaterial;

    [SerializeField]
    Material redMaterial;

    [SerializeField]
    MeshRenderer meshRenderer;

    bool isGreen = true;
    Action onInteractionComplete;

    [SerializeField]
    float timer = 0.5f;

    bool isActive = false;
    GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractibleAtGridPosition(gridPosition, this);

        SetColorGreen();
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }

    void SetColorRed()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        isActive = true;
        this.onInteractionComplete = onInteractionComplete;

        if (isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
}
