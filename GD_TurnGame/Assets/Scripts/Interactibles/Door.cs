using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    [Header("References")]
    [SerializeField]
    Animator animator;

    [Header("Settings")]
    [SerializeField]
    bool isOpen;

    [SerializeField]
    float interactionDuration = 1f;

    GridPosition gridPosition;
    Action onInteractionComplete;

    bool isActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractibleAtGridPosition(gridPosition, this);

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
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

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
        animator.SetBool("IsOpen", true);
    }
    void CloseDoor()
    {
        isOpen = false;
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        animator.SetBool("IsOpen", false);
    }
}
