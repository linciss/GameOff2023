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

    private Dictionary<Direction, Cell> neighbours;

    public Cell(Vector3I position)
    {
        this.position = position;
        initNeibhours();
    }
    public Cell(ICellItem cellItem, Vector3I position)
    {
        this.position = position;
        this.cellItem = cellItem;
        initNeibhours();
    }

    private void initNeibhours()
    {
        neighbours = new Dictionary<Direction, Cell>();
        
        neighbours[Direction.NORTH] = GridSystem.getCell(position + Vector3I.Forward);
        neighbours[Direction.SOUTH] = GridSystem.getCell(position + Vector3I.Back);
        neighbours[Direction.EAST] = GridSystem.getCell(position + Vector3I.Right);
        neighbours[Direction.WEST] = GridSystem.getCell(position + Vector3I.Left);
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
    
    public Dictionary<Direction, Cell> getNeighbours()
    {
        return neighbours;
    }
    
    public Cell getNeighbour(Direction direction)
    {
        return neighbours[direction];
    }

}
