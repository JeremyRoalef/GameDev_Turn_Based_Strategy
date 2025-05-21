using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    CinemachineCamera cinemachineCamera;

    [SerializeField]
    CinemachinePositionComposer cinemachinePositionComposer;

    [SerializeField]
    CinemachineRotationComposer cinemachineRotationComposer;

    [Header("Followoing Settings")]
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float rotationSpeed = 10f;

    [Header("Zoom Settings")]
    [SerializeField]
    float zoomSpeed = 5f;

    [SerializeField]
    float maxCameraYOffset = 2f;

    [SerializeField]
    float minCameraYOffset = 0f;

    [SerializeField]
    [Min(0.01f)]
    float minCameraDistance = 0.01f;

    [SerializeField]
    float maxCameraDistance = 15f;

    private void Update()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoom();
    }

    private void HandleCameraZoom()
    {
        /*
         * let x = current zoom value
         * let a = min zoom value
         * let b = max zoom value
         * 
         * Camera zoom % = (x-a)/(b-a)
         * x = (Camera zoom %)(b-a) + a
         * 
         * Use above to calculate the Y target offset when zooming in and out
         */

        float newZoomPos;
        float zoomRatio;

        if (Input.mouseScrollDelta.y > 0)
        {
            //zoom in
            newZoomPos = cinemachinePositionComposer.CameraDistance - (zoomSpeed * Time.deltaTime);
            cinemachinePositionComposer.CameraDistance = Mathf.Clamp(
                newZoomPos,
                minCameraDistance,
                maxCameraDistance);

            zoomRatio = 
                (float)(cinemachinePositionComposer.CameraDistance - minCameraDistance)/
                (maxCameraDistance-minCameraDistance);
            //Camera y offset
            cinemachinePositionComposer.TargetOffset.y = 
                (zoomRatio * (maxCameraYOffset - minCameraYOffset)) + 
                minCameraYOffset;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            //zoom out
            newZoomPos = cinemachinePositionComposer.CameraDistance + (zoomSpeed * Time.deltaTime);
            cinemachinePositionComposer.CameraDistance = Mathf.Clamp(
                newZoomPos,
                minCameraDistance,
                maxCameraDistance);

            zoomRatio =
                (float)(cinemachinePositionComposer.CameraDistance - minCameraDistance) /
                (maxCameraDistance - minCameraDistance);
            //Camera y offset
            cinemachinePositionComposer.TargetOffset.y =
                (zoomRatio * (maxCameraYOffset - minCameraYOffset)) +
                minCameraYOffset;
        }
    }

    private void HandleCameraRotation()
    {
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

    private void HandleCameraMovement()
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
        }
}