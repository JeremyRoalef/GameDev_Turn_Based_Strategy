using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4f;

    Vector3 targetPos;
    const float nearTargetPosDistance = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
        }
    }

    void Move(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }
}
