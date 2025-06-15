using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    bool invert = false;

    Transform cameraTransform;
    Vector3 directionToFaceCamera;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (invert)
        {
            directionToFaceCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position - directionToFaceCamera);
        }
        else
        {
            transform.LookAt(cameraTransform.position);
        }
    }
}
