using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField]
    Unit selectedUnit;

    [SerializeField]
    LayerMask unitLayerMask;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (TryHandleUnitSelection())
        {

        }
        else
        {
            selectedUnit.Move(MouseWorld.GetMouseWorldPosition());
        }
    }

    bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit newUnit))
            {
                selectedUnit = newUnit;
                return true;
            }
        }
        else
        {
            return false;
        }

        return false;
    }
}
