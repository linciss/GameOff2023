using Godot;

namespace GameOff2023.entities.placeable;

public interface ICellItem
{
    public Node3D getNode();
    
    public Cell getCell();
}