using Godot;
using System;
using GameOff2023.entities;
using GameOff2023.entities.placeable;

public partial class Cell : Node3D
{

    private ICellItem cellItem;
    private Vector3I position;

    public Cell(Vector3I position)
    {
        this.position = position;
    }
    public Cell(ICellItem cellItem, Vector3I position)
    {
        this.position = position;
        this.cellItem = cellItem;
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
    
    
    
}
