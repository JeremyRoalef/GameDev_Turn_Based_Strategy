using Unity.VisualScripting;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField]
    LayerMask mousePlaneLayer;

    //Static reference to the mouse position
    static MouseWorld instance;

    private void Awake() 
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
