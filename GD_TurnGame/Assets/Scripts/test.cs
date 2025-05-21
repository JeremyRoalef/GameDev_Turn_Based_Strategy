using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    Unit unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit.GetMoveAction().GetValidActionGridPositionList();
        }
    }
}
