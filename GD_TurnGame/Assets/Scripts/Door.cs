using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractible
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    bool isOpen;
    GridPosition gridPosition;

    [SerializeField]
    float timer = 1f;

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

        timer -= Time.deltaTime;

        if (timer <= 0)
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
