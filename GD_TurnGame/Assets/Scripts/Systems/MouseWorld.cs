using Unity.VisualScripting;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    //Static reference to the mouse position
    static MouseWorld instance;

    [SerializeField]
    LayerMask mousePlaneLayer;

    private void Awake() 
    {
        //Set singleton
        instance = this;
    }


    /// <summary>
    /// Get the position of the mouse in the world
    /// </summary>
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, instance.mousePlaneLayer);
        return raycastHit.point;
    }
}
