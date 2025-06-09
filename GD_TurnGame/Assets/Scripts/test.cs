using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            GridPosition startgridposition = new GridPosition(0, 0);

            List<GridPosition> gridpositionList = Pathfinding.Instance.FindPath(startgridposition, mouseGridPosition);
            for (int i = 0; i < gridpositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridpositionList[i]), 
                    LevelGrid.Instance.GetWorldPosition(gridpositionList[i+1]), 
                    Color.white, 
                    10);
            }
        }
    }
}
