using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    Animator unitAnimator;

    [SerializeField]
    float moveSpeed = 4f;

    [SerializeField]
    float rotateSpeed = 10f;

    GridPosition gridPosition;
    Vector3 targetPos;
    const float nearTargetPosDistance = 0.1f;

    private void Awake()
    {
        targetPos = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    void Update()
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPos)
    {
        unitAnimator.SetBool("IsWalking", true);
        this.targetPos = targetPos;
    }
}
