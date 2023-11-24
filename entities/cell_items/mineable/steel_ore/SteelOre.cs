using GameOff2023.entities.placeable;
using Godot;

namespace GameOff2023.entities.cell_items.steel_ore;

public class SteelOre : ICellItem, IOre
{
    Node3D node;
    Cell cell;

    public SteelOre(Node3D node, Cell cell)
    {
        this.node = node;
        this.cell = cell;
    }
    
    public Node3D getNode()
    {
        return node;
    }

    public Cell getCell()
    {
        return cell;
    }

    public ItemEnum getOreType()
    {
        return ItemEnum.RawSteel;
    }
}