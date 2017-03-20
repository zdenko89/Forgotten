using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Cell : MonoBehaviour, IGraphNode
{
    [HideInInspector]
    [SerializeField]
    private Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }

    //Checks if tile is occupied
    public bool IsTaken;
    //Cost of moving through a tile
    public int EnergyCost;

    public event EventHandler CellClicked;
    public event EventHandler CellHighlighted;
    public event EventHandler CellDehighlighted;

    protected virtual void OnMouseEnter()
    {
        if (CellHighlighted != null)
            CellHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {    
        if (CellDehighlighted != null)
            CellDehighlighted.Invoke(this, new EventArgs());
    }
    void OnMouseDown()
    {
        if (CellClicked != null)
            CellClicked.Invoke(this, new EventArgs());
    }

    public abstract int GetDistance(Cell other);

    public abstract List<Cell> GetNeighbours(List<Cell> cells);
      
    public abstract Vector3 GetCellDimensions(); //Cell dimensions for grid generators.

    ///  Checks if the selected unit can reach it.
    public abstract void MarkAsReachable();

    public abstract void MarkAsPath();

    // Mark the cell as highlighted
    public abstract void MarkAsHighlighted();

    public abstract void UnMark();

    public int GetDistance(IGraphNode other)
    {
        return GetDistance(other as Cell);
    }
}