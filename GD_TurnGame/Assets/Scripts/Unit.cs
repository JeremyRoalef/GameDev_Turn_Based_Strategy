using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4f;

    Vector3 targetPos;
    const float nearTargetPosDistance = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) > nearTargetPosDistance)
        {
            //Smooth move to position
            Vector3 moveDir = (targetPos - transform.position).normalized;
            transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
        else 
        {
            //Snap to position
            transform.position = targetPos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetMouseWorldPosition());
        }
    }

    void Move(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
