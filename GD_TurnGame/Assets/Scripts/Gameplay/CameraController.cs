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

        float zoomDir = InputManager.Instance.GetCameraZoomAmount();
        if (zoomDir == 0) return;

        //Zoom in/out
        newZoomPos = cinemachinePositionComposer.CameraDistance + (zoomDir * zoomSpeed * Time.deltaTime);
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

    private void HandleCameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleCameraMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        Vector3 moveVector = (
            //TakeAction forward/backward if there is a move input
            transform.forward * inputMoveDir.y) +
            //TakeAction right/left if there is a move input
            (transform.right * inputMoveDir.x);

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}