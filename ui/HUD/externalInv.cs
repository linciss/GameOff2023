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
        for (int i = 0; i < chestSlots.Length; i++)
        {
            chestSlots[i].update(null);
        }

        // Check if the scene is still valid
        if (!IsInstanceValid(this))
        {
            GD.Print("externalInv scene is not valid.");
            return;
        }

        foreach (var kvp in chestInventory.GetInventory())
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;

            Slot targetSlot = FindSlot(item, chestSlots);

            if (targetSlot != null)
            {
                GD.Print($"Updating chest slot {itemEnum} with {item.quantity} items. itemIndex: {targetSlot.GetIndex()}");
                targetSlot.update(item);
                targetSlot.SetCurrentSlotItem(item);
            }
        }
    }

    public void UpdatePlayerSlots()
    {
        
        for (int i = 0; i < playerSlots.Length; i++)
        {
            playerSlots[i].update(null);
        }
        
        foreach (var kvp in playerInventory.GetInventory())
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;

            
            Slot targetSlot = FindSlot(item, playerSlots);
            
            if (targetSlot != null)
            {
                GD.Print($"Updating player slot {itemEnum} with {item.quantity} items. itemIndex: {targetSlot.GetIndex()}");
                targetSlot.update(item);
                targetSlot.SetCurrentSlotItem(item);
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
                if (chestInventory.CanAddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity))
                {
                    chestInventory.AddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
                    playerInventory.RemoveItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
                    playerInventory.PrintAllItems();
                    int childCount = chestGridContainer.GetChildCount();

                    if (childCount > 0)
                    {
                        Control lastItem = chestGridContainer.GetChild(childCount - 1) as Control;
                        chestGridContainer.MoveChild(lastItem, targetIndex);
                    }
                    RemoveItemFromSlot(grabbedItem);
                    UpdateChestSlots();
                    UpdatePlayerSlots();
                }
            }
            else if (grabbedItem.GetParent().Name == "ExternalGridContainer")
            {
                if(playerInventory.CanAddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity))
                {
                    playerInventory.AddItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
                    chestInventory.GetInventory().Remove(grabbedItem.getItem().type);
                    chestInventory.RemoveItem(grabbedItem.getItem().type, grabbedItem.getItem().quantity);
                    int childCount = playerGridContainer.GetChildCount();

                    if (childCount > 0)
                    {
                        Control lastItem = playerGridContainer.GetChild(childCount - 1) as Control;

                        playerGridContainer.MoveChild(lastItem, targetIndex);
                    }
                    UpdateChestSlots();
                    UpdatePlayerSlots();
                }
            }
        }
        else
        {
            //swapping within the same grid container
            grabbedItem.GetParent().MoveChild(grabbedItem, targetIndex);
            hoverOverItem.GetParent().MoveChild(hoverOverItem, grabbedIndex);
        }
    }
}
}