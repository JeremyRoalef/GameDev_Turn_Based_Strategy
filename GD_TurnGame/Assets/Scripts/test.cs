using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField]
    Transform gridDebugObjectPrefab;

    GridSystem grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = new GridSystem(10, 10, 2);
        grid.CreateDebugObjects(gridDebugObjectPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(grid.GetGridPosition(MouseWorld.GetMouseWorldPosition()));
    }
}
