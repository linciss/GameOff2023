using Godot;
using System;
using GameOff2023.entities;
using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot.Collections;

public partial class Cell : Node3D
{

    private ICellItem cellItem;
    private Vector3I position;

    private Dictionary<Direction, Vector3I> neighbours;

    public Cell(Vector3I position)
    {
        this.position = position;
        initNeighbours();
    }

    public Cell(ICellItem cellItem, Vector3I position)
    {
        this.cellItem = cellItem;
        this.position = position;
        initNeighbours();
    }
    
    private void initNeighbours()
    {
        neighbours = new Dictionary<Direction, Vector3I>();

        neighbours.Add(Direction.NORTH, Vector3I.Forward);
        neighbours.Add(Direction.SOUTH, Vector3I.Back);
        neighbours.Add(Direction.EAST, Vector3I.Right);
        neighbours.Add(Direction.WEST, Vector3I.Left);
    }

    public bool hasCellItem()
    {
        return cellItem != null;
    }

    // Getters and Setters
    public ICellItem getCellItem()
    {
        return cellItem;
    }
    
    public Vector3I getPosition()
    {
        return position;
    }
    
    public void setCellItem(ICellItem placeable)
    {
        this.cellItem = placeable;
    }
    
    public void setPosition(Vector3I position)
    {
        this.position = position;
    }
    
    public Dictionary<Direction, Vector3I> getNeighbours()
    {
        return neighbours;
    }
    
    public Cell getNeighbour(Direction direction)
    {
        return GridSystem.getCell(neighbours[direction]);
    }

}
