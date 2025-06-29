#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {        
        Vector2 inputMoveDir = new Vector2(0, 0);

#if USE_NEW_INPUT_SYSTEM
        inputMoveDir = playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1f;
        }
#endif
        return inputMoveDir;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0f;
#if USE_NEW_INPUT_SYSTEM
        rotateAmount = playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount = -1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = 1f;
        }
#endif
        return rotateAmount;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;
#if USE_NEW_INPUT_SYSTEM
        zoomAmount = playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = 1f;
        }
#endif
        return zoomAmount;
    }
}
