using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    GridSystem<GridObject> gridSystem;
    GridPosition gridPosition;

    public List<Unit> units = new List<Unit>();

    Door door;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in units)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        units.Remove(unit);
    }

    public List<Unit> GetUnits()
    {
        return units;
    }

    public bool HasAnyUnit()
    {
        return units.Count > 0;
    }

    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return units[0];
        }
        else
        {
            return null;
        }
    }

    public Door GetDoor()
    {
        return door;
    }
    public void SetDoor(Door door)
    {
        this.door = door;
    }
}
