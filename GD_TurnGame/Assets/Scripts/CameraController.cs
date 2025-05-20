using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float rotationSpeed = 10f;
    private void Update()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1f;
        }

        Vector3 moveVector = (
            //Move forward/backward if there is a move input
            transform.forward * inputMoveDir.z) + 
            //Move right/left if there is a move input
            (transform.right * inputMoveDir.x);

        transform.position += moveVector * moveSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = 1f;
        }
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
}
