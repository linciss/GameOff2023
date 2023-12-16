using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GameOff2023.entities;
using GameOff2023.entities.chest;
using GameOff2023.entities.placeable;
using GameOff2023.globals.Inventory;

public partial class externalInv : Control, IItemHolder
{
    [Export]private Slot[] playerSlots;
    [Export]private Slot[] chestSlots;
    [Export] private GridContainer playerGridContainer;
    [Export]private GridContainer chestGridContainer;
    private Slot grabbedItem { get; set; }
    private Slot hoverOverItem { get; set; }
    IInventory playerInventory;
    IInventory chestInventory;
    Vector2 mouseOffset = new  Vector2(550, 324);
    private Vector2 lastMousePos;
    private Player player;
    public override void _Ready()
    {
        player = player_instance.GetPlayer();
        playerInventory = player.inventory;
        GD.Print("Wakey wakey eggs and bakey!");
        
        if (chestGridContainer != null)
        {
            chestSlots= new Slot[chestGridContainer.GetChildCount()];
			
            for (int i = 0; i < chestGridContainer.GetChildCount(); i++)
            {
                Node childNode = chestGridContainer.GetChild(i);
                chestSlots[i] = childNode as Slot;
            }
        }
        if (playerGridContainer != null)
        {
            playerSlots = new Slot[playerGridContainer.GetChildCount()];
			
            for (int i = 0; i < playerGridContainer.GetChildCount(); i++)
            {
                Node childNode = playerGridContainer.GetChild(i);
                playerSlots[i] = childNode as Slot;
            }
        }
        
        Visible = false;
        UpdateChestSlots();
        UpdatePlayerSlots();
        Array.Sort(chestSlots, (a, b) => a.GetIndex().CompareTo(b.GetIndex()));
        Array.Sort(playerSlots, (a, b) => a.GetIndex().CompareTo(b.GetIndex()));
    }
    
    public void SetChestInventory(IInventory newInventory, string entity)
    {
        if (chestInventory != null)
        {
            chestInventory.InventoryChanged -= UpdateChestSlots;
        }

        chestInventory = newInventory;

        if (chestInventory != null)
        {
            chestInventory.InventoryChanged += UpdateChestSlots;
            UpdateChestSlots();
        }
    }
    
    
    public void UpdateChestSlots()
    {
        // Wipe invUI
        foreach (var slot in chestSlots)
        {
            slot.update(null);
        }

        foreach (var chestSlot in chestSlots)
        {
            Item item = chestInventory.GetItemAtIndex(chestSlot.GetIndex());
            if (item != null)
            {
                chestSlot.update(item);
                chestSlot.SetCurrentSlotItem(item);
            }
        }
    }

    public void UpdatePlayerSlots()
    {
        // Wipe invUI
        foreach (var slot in playerSlots)
        {
            slot.update(null);
        }

        foreach (var playerSlot in playerSlots)
        {
            Item item = playerInventory.GetItemAtIndex(playerSlot.GetIndex());
            if (item != null)
            {
                playerSlot.update(item);
                playerSlot.SetCurrentSlotItem(item);
            }
        }
    }

    private bool grabbed = false;
    public override void _Process(double delta)
    {
        //Item moving throughout inv logic!
        if (grabbedItem is not null && grabbedItem.getItem() is not null)
        {
            if (Input.IsActionJustPressed("left_mouse_click"))
            {
                SetGrabbedState(true);
                lastMousePos = GetGlobalMousePosition();
            }

            if (lastMousePos.DistanceTo(GetGlobalMousePosition()) > 2)
            {
                
                if (Input.IsActionPressed("left_mouse_click"))
                { 
                    grabbed = true;
                }
                if (Input.IsActionJustReleased("left_mouse_click"))
                {
                    grabbed = false;
                    swapItems();
                    SetGrabbedState(false);
                }
            }
        }
        moveSprite();
    }
    
    public void SetGrabbedState(bool state)
    {
        foreach (var slot in playerSlots)
        {
            slot.SetGrabbedState(state);
        }
        foreach (var slot in chestSlots)
        {
            slot.SetGrabbedState(state);
        }
    }
    
     private Slot FindSlot(Item item, Slot[] slots)
    {
        foreach (var slot in slots)
        {
            if (slot.GetCurrentSlotItem() == null || slot.GetCurrentSlotItem().Equals(item))
            {
                return slot;
            }
        }
        return null;
    }
   

    public void SetVisibility(bool state)
    {
        Visible = state;
    }
    
    public Slot GetGrabbedItem()
    {
        return grabbedItem;
    }
    
    public void RemoveItemFromSlot(Slot slot)
    {
        slot.update(null);
        grabbedItem = null;
    }
    
    public void AddItemToSlot(Slot slot)
    {
        slot.update(grabbedItem.getItem());
    }
    
    public void SetGrabbedItem(Slot slot)
    {
        grabbedItem = slot;
    }

    public void SetHoveredItem(Slot slot)
    {
        hoverOverItem = slot;
    }

    private int slotIndex;
    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }
    private int currentSlotIndex;
    public void SetCurrentSlotIndex(int index)
    {
        currentSlotIndex = index;
    }
    
    private Texture2D heldItemTexture;
    [Export]private Sprite2D heldItemSprite;
    public void moveSprite()
    {
        if (grabbed)
        {
            heldItemTexture = GD.Load<Texture2D>(grabbedItem.getItem().image);
            float desiredWidth = 40.0f;
            float desiredHeight = 40.0f;
            float originalWidth = heldItemTexture.GetWidth();
            float originalHeight = heldItemTexture.GetHeight();
			
            heldItemSprite.Texture = heldItemTexture;
            heldItemSprite.Scale = new Vector2(desiredWidth / originalWidth, desiredHeight / originalHeight);
            heldItemSprite.Texture = heldItemTexture;
            heldItemSprite.Position = GetGlobalMousePosition() + new Vector2(0, 50);
            heldItemSprite.Visible = true;
        }
        else
        {
            heldItemTexture = null;
            heldItemSprite.Texture = heldItemTexture;   
            heldItemSprite.Visible = false;
        }
        
    }
    public void swapItems()
    {
        int grabbedIndex = currentSlotIndex;
        int targetIndex = slotIndex;

        GD.Print("SWAPPED");
        //swapping between two different grid containers
        if (grabbedItem != null && hoverOverItem != null)
        {
            if (grabbedItem.GetParent() != hoverOverItem.GetParent())
            {
                if (grabbedItem.GetParent().Name == "PlayerGridContainer")
                {
                    TransferItems(playerInventory, chestInventory, chestGridContainer, targetIndex);
                }else if (grabbedItem.GetParent().Name == "ExternalGridContainer")
                {
                    TransferItems(chestInventory, playerInventory, playerGridContainer, targetIndex);
                }
                UpdateChestSlots();
                UpdatePlayerSlots();
            }else
            {
                //swapping within the same grid container
                grabbedItem.GetParent().MoveChild(grabbedItem, targetIndex);
                hoverOverItem.GetParent().MoveChild(hoverOverItem, grabbedIndex);
            }
        }
    }
    public void TransferItems(IInventory fromInventory, IInventory toInventory, GridContainer toGrid, int targetIndex)
    {
        if (toInventory.CanAddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity))
        {
            int transferQuantity = toInventory.CalculateTransferQuantity(grabbedItem.getItem().type, grabbedItem.getItem().quantity);

            toInventory.AddItem(grabbedItem.getItem().type, transferQuantity);
            fromInventory.RemoveItem(grabbedItem.getItem().type, transferQuantity);
            int childCount = toGrid.GetChildCount();
            if (childCount > 0)
            {
                Control lastItem = toGrid.GetChild(childCount - 1) as Control;

                toGrid.MoveChild(lastItem, targetIndex);
            }
            AddItemToSlot(grabbedItem);
        }
    }
} 
   