using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// Base class for all units in the game.
public abstract class Unit : MonoBehaviour
{
    public event EventHandler UnitClicked;  
    public event EventHandler UnitSelected;
    public event EventHandler UnitDeselected;

    public event EventHandler UnitHighlighted;
    public event EventHandler UnitDehighlighted;
    public event EventHandler<AttackEventArgs> UnitAttacked;
    public event EventHandler<AttackEventArgs> UnitDestroyed;
    public event EventHandler<MovementEventArgs> UnitMoved;

    public UnitState UnitState { get; set; }
    public void SetState(UnitState state)
    {
        UnitState.MakeTransition(state);
    }


    public int TotalHP { get; private set; }
    protected int TotalEnergy;
    protected int TotalAttackPoints;

    public Cell Cell { get; set; } //Cell that a unit is currently in

    public int HP; //HitPoints
    public int AttackRange; //Range of the attacks
    public int AttackBase;//Attack Factor
    public int Defence;//DefenceFactor
    public int Energy;//MovementPoints - How much unit can move
    public float MovementSpeed; 
    public int AttackPoints;//Amount of attacks unit can perform
    public int PlayerNumber;

    public bool isMoving { get; set; }

    private static IPathfinding _pathfinder = new AStarPathfinding(); //A* pathfinding algorithm

    public virtual void Initialize()
    {

        UnitState = new UnitStateNormal(this);

        TotalHP = HP;
        TotalEnergy = Energy;
        TotalAttackPoints = AttackPoints;
    }

    protected virtual void OnMouseDown()
    {
        if (UnitClicked != null)
            UnitClicked.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseEnter()
    {
        if (UnitHighlighted != null)
            UnitHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {
        if (UnitDehighlighted != null)
            UnitDehighlighted.Invoke(this, new EventArgs());
    }

    //Start of turn
    public virtual void OnTurnStart()
    {
        Energy = TotalEnergy; //Energy to use
        AttackPoints = TotalAttackPoints; // AP to use

        SetState(new UnitStateMarkedAsFriendly(this));
    }

    public virtual void OnTurnEnd()
   {

     //   SetState(new UnitStateNormal(this));
    }

    //Once health bar goes down to 0
    protected virtual void OnDestroyed()
    {
        Cell.IsTaken = false;
        MarkAsDestroyed();
        Destroy(gameObject);
    }

    //Unit selection
    public virtual void OnUnitSelected()
    {
        SetState(new UnitStateMarkedAsSelected(this));
        if (UnitSelected != null)
            UnitSelected.Invoke(this, new EventArgs());
    }

    //Unit Deselection
    public virtual void OnUnitDeselected()
    {
        SetState(new UnitStateMarkedAsFriendly(this));
        if (UnitDeselected != null)
            UnitDeselected.Invoke(this, new EventArgs());
    }

    //Checks if unit can attack
    public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange)
            return true;

        return false;
    }
    
    //Amount of damage done
    public virtual void DealDamage(Unit other)
    {
        if (isMoving)
            return;
        if (AttackPoints == 0)
            return;
        if (!IsUnitAttackable(other, Cell))
            return;

        MarkAsAttacking(other);
        AttackPoints--;
        other.Defend(this, AttackBase);

        if (AttackPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            Energy = 0;
        }  
    }
    
    protected virtual void Defend(Unit other, int damage)
    {
        MarkAsDefending(other);
        HP -= Mathf.Clamp(damage - Defence, 1, damage);  //Damage is calculated by subtracting attack factor of attacker and defence factor of defender. If result is below 1, it is set to 1.
                                                                      //This behaviour can be overridden in derived classes.
        if (UnitAttacked != null)
            UnitAttacked.Invoke(this, new AttackEventArgs(other, this, damage));

        if (HP <= 0)
        {
            if (UnitDestroyed != null)
                UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, damage));
            OnDestroyed();
        }
    }

    public virtual void Move(Cell destinationCell, List<Cell> path)
    {
        if (isMoving)
            return;

        var totalEnergyCost = path.Sum(h => h.EnergyCost);
        if (Energy < totalEnergyCost)
            return;

        Energy -= totalEnergyCost;

        Cell.IsTaken = false;
        Cell = destinationCell;
        destinationCell.IsTaken = true;

        if (MovementSpeed > 0)
            StartCoroutine(MovementAnimation(path));
        else
            transform.position = Cell.transform.position;

        if (UnitMoved != null)
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));    
    }
    protected virtual IEnumerator MovementAnimation(List<Cell> path)
    {
        isMoving = true;

        path.Reverse();
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * MovementSpeed);
                yield return 0;
            }
        }

        isMoving = false;
    }

    //Checks if player can move
    public virtual bool IsCellMovableTo(Cell cell)
    {
        return !cell.IsTaken;
    }
    
    public virtual bool IsCellTraversable(Cell cell)
    {
        return !cell.IsTaken;
    }

    // Checks that the unit is capable of moving to.
    public List<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        var ret = new List<Cell>();
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= Energy);

        var traversableCells = cells.FindAll(c => IsCellTraversable(c) && c.GetDistance(Cell) <= Energy);
        traversableCells.Add(Cell);

        foreach (var cellInRange in cellsInMovementRange)
        {
            if (cellInRange.Equals(Cell)) continue;

            var path = FindPath(traversableCells, cellInRange);
            var pathCost = path.Sum(c => c.EnergyCost);
            if (pathCost > 0 && pathCost <= Energy)
                ret.AddRange(path);
        }
        return ret.FindAll(IsCellMovableTo).Distinct().ToList();
    }

    public List<Cell> FindPath(List<Cell> cells, Cell destination)
    {
        return _pathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
    }

    // Graph representation of cell grid for pathfinding.
    protected virtual Dictionary<Cell, Dictionary<Cell, int>> GetGraphEdges(List<Cell> cells)
    {
        Dictionary<Cell, Dictionary<Cell, int>> ret = new Dictionary<Cell, Dictionary<Cell, int>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell.Equals(Cell))
            {
                ret[cell] = new Dictionary<Cell, int>();
                foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                {
                    ret[cell][neighbour] = neighbour.EnergyCost;
                }
            }
        }
        return ret;
    }

    public abstract void MarkAsDefending(Unit other);
    public abstract void MarkAsAttacking(Unit other);
    public abstract void MarkAsDestroyed();
    public abstract void MarkAsFriendly();
    public abstract void MarkAsReachableEnemy();
    public abstract void MarkAsSelected();
    public abstract void MarkAsFinished();
    public abstract void UnMark();
}

public class MovementEventArgs : EventArgs
{
    public Cell OriginCell;
    public Cell DestinationCell;
    public List<Cell> Path;

    public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
    {
        OriginCell = sourceCell;
        DestinationCell = destinationCell;
        Path = path;
    }
}
public class AttackEventArgs : EventArgs
{
    public Unit Attacker;
    public Unit Defender;

    public int Damage;

    public AttackEventArgs(Unit attacker, Unit defender, int damage)
    {
        Attacker = attacker;
        Defender = defender;

        Damage = damage;
    }
}

//State of the unit after it is finished with its turn
public abstract class UnitState
{
    protected Unit _unit;

    public UnitState(Unit unit)
    {
        _unit = unit;
    }

    public abstract void Apply();
    public abstract void MakeTransition(UnitState state);
}

public class UnitStateMarkedAsFinished : UnitState
{
    public UnitStateMarkedAsFinished(Unit unit) : base(unit)
    {
    }

    public override void Apply()
    {
        _unit.MarkAsFinished();
    }

    public override void MakeTransition(UnitState state)
    {
        if (state is UnitStateNormal)
        {
            state.Apply();
            _unit.UnitState = state;
        }
    }
}
public class UnitStateMarkedAsFriendly : UnitState
{
    public UnitStateMarkedAsFriendly(Unit unit) : base(unit)
    {

    }

    public override void Apply()
    {
        _unit.MarkAsFriendly();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}
public class UnitStateMarkedAsReachableEnemy : UnitState
{
    public UnitStateMarkedAsReachableEnemy(Unit unit) : base(unit)
    {
    }

    public override void Apply()
    {
        _unit.MarkAsReachableEnemy();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}
public class UnitStateMarkedAsSelected : UnitState
{
    public UnitStateMarkedAsSelected(Unit unit) : base(unit)
    {
    }

    public override void Apply()
    {
        _unit.MarkAsSelected();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}
public class UnitStateNormal : UnitState
{
    public UnitStateNormal(Unit unit) : base(unit)
    {
    }

    public override void Apply()
    {
        _unit.UnMark();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}









