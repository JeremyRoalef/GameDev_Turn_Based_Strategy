using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    bool invert = false;

    Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (invert)
        {
            Vector3 dirToCamera = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position - dirToCamera);
        }
        else
        {
            transform.LookAt(cameraTransform.position);
        }
    }
}
