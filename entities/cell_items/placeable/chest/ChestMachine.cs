using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot;

namespace GameOff2023.entities.chest;

public partial class ChestMachine : Node3D, ICellItem, IPlaceable, IMachineInput
{
    private Cell cell;
    private Node3D node;
    private InventoryAPI inventory;

    public ChestMachine(Cell cell, SceneTree tree)
    {
        node = this;
        this.cell = cell;
        inventory = new InventoryAPI();
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/cell_items/placeable/chest/chest_machine.tscn");
        AddChild(prefab.Instantiate());
        tree.CurrentScene.GetNode<Node3D>("/root/Asteroid/PlayerPlaceable/").AddChild(this);

    }
    
    public Node3D getNode()
    {
        return node;
    }

    public Cell getCell()
    {
        return cell;
    }

    public PlaceableType getType()
    {
        return PlaceableType.Chest;
    }

    public void tick()
    {
        //Iterate over neibhour cells
        foreach (Vector3I neighbour in cell.getNeighbours().Values)
        {
            // Cell targetCell = GridSystem.getCell(neighbour);
            // //If the cell has a cell item
            // if (!targetCell.hasCellItem()) return;
            //
            // if (!(targetCell.getCellItem() is IMachineInput)) return;
            //
            // IMachineOutput machineOutput = (IMachineOutput) targetCell.getCellItem();
            //
            // //
            // Item item = inventory.TakeFirstItem();
            // if (!(machineOutput.canOutput(item, this))) return;
            //
            // inventory.RemoveItem(item.GetType(), 1);
            // machineOutput.output(item);
        }
    }

    public bool canInput(Item item, ICellItem cellItem)
    {
        return inventory.CanAddItem(item.GetType(), item.quantity);
    }

    public void input(Item item)
    {
        inventory.AddItem(item.GetType(), item.quantity);
    }
    
    
    
}