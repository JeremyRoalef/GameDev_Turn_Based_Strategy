using System;
using UnityEngine;

public class CameraManager : MonobehaviourEventListener
{
    [SerializeField]
    GameObject actionCameraObj;

    [Header("Shoot Action")]
    [SerializeField]
    Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

    [SerializeField]
    float shoulderOffsetAmount = 0.5f;

    private void Start()
    {
        SubscribeEvents();
        HideActionCamera();
    }



    protected override void SubscribeEvents()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    protected override void UnsubscribeEvents()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }



    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
            default:
                break;
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                //Get units
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                //Compute camera position and rotation
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                Vector3 shoulderOffset = Quaternion.Euler(0f, 90f, 0f) * shootDir * shoulderOffsetAmount;
                Vector3 cameraPos = 
                    shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset - shootDir;

                //Set action camera position and rotation
                actionCameraObj.transform.position = cameraPos;
                actionCameraObj.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                //Show the camera
                ShowActionCamera();
                break;
            default:
                break;
        }
    }



    void ShowActionCamera()
    {
        actionCameraObj.SetActive(true);
    }

    void HideActionCamera()
    {
        actionCameraObj?.SetActive(false);
    }
}
