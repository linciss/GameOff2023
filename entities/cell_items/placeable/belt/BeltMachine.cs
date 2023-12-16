using System;
using GameOff2023.entities.placeable;
using GameOff2023.random;
using Godot;

namespace GameOff2023.entities.belt;

public partial class BeltMachine : Node3D, ICellItem, IPlaceable, IMachineInput
{
    private Cell cell;
    private Item currItem;
    private Sprite3D visualItem;
    
    private bool waitingToPlace = false;
    private bool visualReachedMax = false;
    private Vector3 maxVisualPos;

    public BeltMachine(Cell cell, Vector3 rotation, SceneTree tree)
    {
        this.cell = cell;
        // inventory = new InventoryAPI(1);
        
        RotationDegrees = rotation;
        
        PackedScene prefab = (PackedScene)ResourceLoader.Load("res://entities/cell_items/placeable/belt/belt_machine.tscn");
        AddChild(prefab.Instantiate());
        tree.CurrentScene.GetNode<Node3D>("/root/Asteroid/PlayerPlaceable/").AddChild(this);
    
        helperPlaceIndicator(Vector3.Forward);
        
        // GD.Print("BELT PLACED AT: "+ cell.getPosition());
        // inventory.AddItem(ItemEnum.RawSteel, 5);
        // inventory.PrintAllItems();
        
        // Prepare visual item
        Sprite3D sprite = new Sprite3D();
        sprite.RotationDegrees = new Vector3(90, 0, 0);
        sprite.Scale = new Vector3(0.1f, 0.1f, 0.1f);
        sprite.Position = new Vector3(0, 0.2f, 0);
        AddChild(sprite);
        visualItem = sprite;
        
        //Calculate max visual item pos
        maxVisualPos = getBeltDirection() - getBeltDirection() * 0.5f;
    }

    public override void _Process(double delta)
    {
        Vector3 beltDirection = getBeltDirection();
        if (waitingToPlace)
        {
            // Check if the position has reached with an error of 0.01
            if (visualReachedMax) return;
            if (visualItem.Position.DistanceTo(maxVisualPos) < 0.01)
            {
                waitingToPlace = true;
                visualReachedMax = true;
                visualItem.Position = maxVisualPos;
                return;
            }
        }
        visualItem.Position = beltDirection * GridSystem.getProgressToTick() - beltDirection * 0.5f;
    }

    public void tick()
    {
        GD.PrintErr("BELT TICK!!!!!!!! "+ cell.getPosition());
        waitingToPlace = true;
        
        Vector3 targetCellPos = cell.getPosition() + GetVector3InFront(RotationDegrees.Y);
        Vector3I gridPosition = GridSystem.translateToGridPos(targetCellPos);
        Cell targetCell = GridSystem.getCell(gridPosition);

        if (!targetCell.hasCellItem()) return;

        if (!(targetCell.getCellItem() is IMachineInput)) return;
        
        IMachineInput machineInput = (IMachineInput)targetCell.getCellItem();
        
        if (currItem == null) return;

        if (!machineInput.canInput(currItem.GetType(), 1, this)) return;

        machineInput.input(currItem.GetType(), 1);
        
        if (!currItem.RemoveQuantity(1)) currItem = null;

        if (currItem == null) visualItem.Texture = null;

        waitingToPlace = false;
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
        if (currItem != null) 
        {
            GD.PrintErr("belt inv cant take");
            return false;
        }
        
        GD.Print("inv can add");

        // if (!(cellItem is IPlaceable placeable)) return false;
        //
        // GD.Print("is placeable");

        bool result = isValidChestPlacement(getCell().getPosition(), cellItem.getCell().getPosition());
        
        GD.Print("belt caninput: "+result);
        
        return result;
    }

    public void input(ItemEnum item, int quantity)
    {
        currItem = ItemFactory.newItem(item);
        currItem.quantity = quantity;

        visualItem.Texture = (Texture2D)ResourceLoader.Load(ItemFactory.allItems[item].image);

        GD.PrintErr("BELT INV:,");
    }


    public Vector3 GetVector3InFront(float angle)
    {
        angle = Math.Abs(angle);
        if (angle == 0) return new Vector3(0, 0, -1);
        if (angle == 90) return new Vector3(1, 0, 0);
        if (angle == 180) return new Vector3(0, 0, 1);
        if (angle == 270) return new Vector3(-1, 0, 0);
        return new Vector3(0, 0, 1);
        
    }
    
    public bool isValidChestPlacement(Vector3I beltPos, Vector3I chestPos)
    {
        Vector3I difference = chestPos-beltPos;
        
        GD.Print("difference: "+difference);
        GD.Print("rotation: "+getRotation().Y);
        
        if(difference == new Vector3I(1,0,0) && getRotation().Y == -270 || getRotation().Y == 90)  return true;
        if(difference == new Vector3I(-1,0,0) && getRotation().Y == -90 || getRotation().Y == 270)  return true;
        if(difference == new Vector3I(0,0,-1) && Math.Abs(getRotation().Y) == 180)  return true;
        return difference == new Vector3I(0, 0, 1) && getRotation().Y == 0;
        
    }
    
    private Vector3 getBeltDirection()
    {
        if(getRotation().Y == -270 || getRotation().Y == 90) return new Vector3I(0,0,-1);
        if(getRotation().Y == -90 || getRotation().Y == 270) return new Vector3I(0,0,-1);
        if(Math.Abs(getRotation().Y) == 180) return new Vector3I(0,0,-1);
        if (getRotation().Y == 0) return new Vector3I(0, 0, -1);

        
        return Vector3.Zero;
    }

    public InventoryAPI getInventory()
    {
        return null;
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
}