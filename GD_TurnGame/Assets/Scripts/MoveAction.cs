using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField]
    Animator unitAnimator;

    [SerializeField]
    float moveSpeed = 4f;

    [SerializeField]
    float rotateSpeed = 10f;

    [SerializeField]
    int maxMoveDistance = 4;

    Unit unit;
    Vector3 targetPos;
    const float nearTargetPosDistance = 0.1f;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPos = transform.position;
    }

    public MoveAction GetMoveAction()
    {
        return GetComponent<MoveAction>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) > nearTargetPosDistance)
        {
            //Smooth move to position
            unitAnimator.SetBool("IsWalking", true);
            Vector3 moveDir = (targetPos - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }
        else
        {
            //Snap to position
            unitAnimator.SetBool("IsWalking", false);
            transform.position = targetPos;
        }
    }

    public void Move(Vector3 targetPos)
    {
        unitAnimator.SetBool("IsWalking", true);
        this.targetPos = targetPos;
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
