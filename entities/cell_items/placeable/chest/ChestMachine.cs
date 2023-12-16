using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot;

namespace GameOff2023.entities.chest;

public partial class ChestMachine : Node3D, ICellItem, IPlaceable, IMachineInput
{
    private Cell cell;
    private Node3D node;
    private InventoryAPI inventory;

    public ChestMachine(Cell cell, Vector3 rotationDeg, SceneTree tree)
    {
        node = this;
        this.cell = cell;
        RotationDegrees = rotationDeg;
        inventory = new InventoryAPI();
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/cell_items/placeable/chest/chest_machine.tscn");
        AddChild(prefab.Instantiate());
        tree.CurrentScene.GetNode<Node3D>("/root/Asteroid/PlayerPlaceable/").AddChild(this);

        inventory.AddItem(ItemEnum.RawSteel, 2);
        inventory.PrintAllItems();
        GD.Print("CHEST PLACED AT: "+ cell.getPosition());
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
    
    public Vector3 getRotation()
    {
        return RotationDegrees;
    }

    public void tick()
    {
        GD.Print("CHEST TICK!!!!!!!! " + cell.getPosition());
        // Iterate over neighboring cells
        foreach (Vector3I neighbor in cell.getNeighbours().Values)
        {
            // Get the neighboring cell
            Cell targetCell = GridSystem.getCell(cell.getPosition() + neighbor);

            // Check if the neighboring cell has a cell item
            if (!targetCell.hasCellItem())
            {
                // Debug message: "Neighbor cell does not have a cell item"
                GD.Print("Neighbor cell does not have a cell  " + neighbor);
                continue;
            }

            // Check if the neighboring cell item is a machine input
            if (!(targetCell.getCellItem() is IMachineInput))
            {
                // Debug message: "Neighbor cell item is not a machine input"
                GD.Print("Neighbor cell item is not a machine input");
                continue;
            }

            // Cast the neighboring cell item to a machine input
            IMachineInput machineInput = (IMachineInput)targetCell.getCellItem();

            // Get the first item from the inventory
            Item item = inventory.TakeFirstItem();

            GD.Print("Checking if item null");
            if (item == null) continue;

                // Debug message: "Getting first item from inventory: " + item
            GD.Print("Getting first item from inventory: " + item);

            // Check if the machine input can input the item
            if (!(machineInput.canInput(item.GetType(), 1, this)))
            {
                // Debug message: "Machine input cannot input item: " + item
                GD.Print("Machine input cannot input item: " + item.GetType());
                continue;
            }

            // Input the item into the machine input
            machineInput.input(item.GetType(), 1);

            // Debug message: "Inputting item: " + item
            GD.Print("Inputting item: " + item.GetType());

            // Remove the item from the inventory
            inventory.RemoveItem(item.GetType(), 1);

            // Print all items in the inventory
            inventory.PrintAllItems();
        }
    }


    public bool canInput(ItemEnum item, int quantity, ICellItem cellItem)
    {
        return inventory.CanAddItem(item, quantity);
    }

    public void input(ItemEnum item, int quantity)
    {
        GD.PrintErr("CHEST RECIEVED");
        inventory.AddItem(item, quantity);
        
        inventory.PrintAllItems();
    }
    
    public InventoryAPI getInventory()
    {
        return inventory;
    }
    
}