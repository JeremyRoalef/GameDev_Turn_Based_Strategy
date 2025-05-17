using UnityEngine;

public class test : MonoBehaviour
{
    GridSystem grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = new GridSystem(10, 10, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(grid.GetGridPosition(MouseWorld.GetMouseWorldPosition()));
    }
}
