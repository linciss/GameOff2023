using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot;

namespace GameOff2023.entities.belt;

public partial class BeltMachine : Node3D, ICellItem, IPlaceable, IMachineInput
{
    private Cell cell;
    private InventoryAPI inventory;

    public BeltMachine(Cell cell, Vector3 rotation, SceneTree tree)
    {
        this.cell = cell;
        inventory = new InventoryAPI(1);
        
        RotationDegrees = rotation;
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/cell_items/placeable/belt/belt_machine.tscn");
        AddChild(prefab.Instantiate());
        tree.CurrentScene.GetNode<Node3D>("/root/Asteroid/PlayerPlaceable/").AddChild(this);
    
        helperPlaceIndicator(GetVector3InFront(RotationDegrees.Y));
        
        GD.Print("BELT PLACED AT: "+ cell.getPosition());
    }
    public Node3D getNode()
    {
        return this;
    }

    public Cell getCell()
    {
        return cell;
    }

    public PlaceableType getType()
    {
        return PlaceableType.Belt;
    }
    
    public Vector3 getRotation()
    {
        return RotationDegrees;
    }

public void tick()
{
    GD.Print("BELT TICK!!!!!!!!");
    // Get the vector3 position of the cell in front of the player
    Vector3 targetCellPos = cell.getPosition() + GetVector3InFront(RotationDegrees.Y);

    // GD.Print: "Getting target cell position: " + targetCellPos
    GD.Print("Getting target cell position: " + targetCellPos);

    // Convert the vector3 position to a grid position
    Vector3I gridPosition = GridSystem.translateToGridPos(targetCellPos);

    // GD.Print: "Converting target cell position to grid position: " + gridPosition
    GD.Print("Converting target cell position to grid position: " + gridPosition);

    // Get the cell at the grid position
    Cell targetCell = GridSystem.getCell(gridPosition);

    // GD.Print: "Getting target cell: " + targetCell
    GD.Print("Getting target cell: " + targetCell);

    // Check if the cell has a cell item
    if (!targetCell.hasCellItem())
    {
        // GD.Print: "Target cell does not have a cell item"
        GD.Print("Target cell does not have a cell item");
        return;
    }

    // Check if the cell item is a machine input
    if (!(targetCell.getCellItem() is IMachineInput))
    {
        // GD.Print: "Target cell item is not a machine input"
        GD.Print("Target cell item is not a machine input");
        return;
    }

    // Cast the cell item to a machine input
    IMachineInput machineInput = (IMachineInput)targetCell.getCellItem();

    // Get the first item from the inventory
    Item item = inventory.TakeFirstItem();

    // GD.Print: "Getting first item from inventory: " + item
    GD.Print("Getting first item from inventory: " + item);

    // Check if the machine input can input the item
    if (!machineInput.canInput(item.GetType(), 1, this))
    {
        // GD.Print: "Machine input cannot input item: " + item
        GD.Print("Machine input cannot input item: " + item);
        return;
    }

    // Input the item into the machine input
    machineInput.input(item.GetType(), 1);

    // Remove the item from the inventory
    inventory.RemoveItem(item.GetType(), 1);

    // Print all items in the inventory
    inventory.PrintAllItems();
}


    //TEMp
    public void helperPlaceIndicator(Vector3 targetCellPos)
    {
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/cell_items/placeable/belt/test.tscn");
        Node3D helperNode = prefab.Instantiate() as Node3D;
        helperNode.Position = targetCellPos;
        AddChild(helperNode);
    }
    
    public bool canInput(ItemEnum item, int quantity, ICellItem cellItem)
    {
        GD.PrintErr(cellItem.GetType() + " TRYING TO INPUT belt");
        // Check if the item is null (valid to input no item)
        if (inventory.CanAddItem(item, quantity)) return true;
        
        GD.Print("inv can add");

        if (!(cellItem is IPlaceable placeable)) return false;
        
        GD.Print("is placeable");

        bool result=RotationUtils.isReverseYRotation(placeable.getRotation().Y, RotationDegrees.Y);
        GD.Print("belt caninput: "+result);
        
        return result;
    }

    public void input(ItemEnum item, int quantity)
    {
        inventory.AddItem(item, quantity);
    }


    public Vector3 GetVector3InFront(float angle)
    {
        angle %= 360f;

        // Handle negative angles
        if (angle > 270f) {
            angle -= 360f; // Convert negative angle to positive equivalent
        }
        
        Vector3 targetCellPos;

        // Determine the direction vector based on the normalized rotation Y
        if (angle <= 90f) {
            targetCellPos = Vector3.Forward; // Front direction
        } else if (angle <= 180f) {
            targetCellPos = Vector3.Right; // Right direction
        } else {
            targetCellPos = Vector3.Back; // Back direction
        }

        return targetCellPos;
    }
}