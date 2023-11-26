using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GameOff2023.entities;
using GameOff2023.entities.chest;
using GameOff2023.entities.placeable;
using GameOff2023.globals.Inventory;

public partial class inv_ui : Control, IItemHolder
{
    [Export]
    public Slot[] slots;
    [Export] private Label test;
    [Export]
    private GridContainer gridContainer;
    private Slot grabbedItem { get; set; }
    private Slot hoverOverItem { get; set; }
    Dictionary<ItemEnum, Item> items;
    IInventory inventory;
    private Texture2D heldItemTexture;
    [Export]
    private Sprite2D heldItemSprite;
    private Vector2 mouseOffset = new Vector2(570, 324);
    private int slotIndex;
    private int currentSlotIndex;
    private Vector2 lastMousePos;
  
    
    public void SetHoveredItem(Slot slot)
    {
        hoverOverItem = slot;
    }
    public void SetGrabbedItem(Slot slot)
    {
        grabbedItem = slot;
    }
    public void SetSlotIndex(int index)
    {
        slotIndex = index;

        foreach (var slot in slots)
        {
            slot.SetSlotIndex(index);
        }
    }
    public void SetCurrentSlotIndex(int index)
    {
        currentSlotIndex = index;
    }

    
    
    public override void _Ready()
    {
        GD.Print("Wakey wakey eggs and bakey!");
        
        
        if (gridContainer != null)
        {
            slots = new Slot[gridContainer.GetChildCount()];
			
            for (int i = 0; i < gridContainer.GetChildCount(); i++)
            {
                Node childNode = gridContainer.GetChild(i);
                slots[i] = childNode as Slot;
               
                GD.Print("slot loaded");
            }
        }
        else
        {
            GD.Print("GridContainer not found.");
        }

        Visible = false;
    }
    
    public void SetInventory(IInventory newInventory, string entity)
    {
        test.Text = entity;
        
        if (inventory != null)
        {
            inventory.InventoryChanged -= UpdateSlots;
        }

        inventory = newInventory;

        if (inventory != null)
        {
            inventory.InventoryChanged += UpdateSlots;
            items= inventory.GetInventory();
            UpdateSlots();
        }
    }


    
    public void UpdateSlots()
    {
        int iteration = 0;
        GD.Print("UPDATE SLOTS");

        items = inventory.GetInventory();

        // Wipe invUI
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].update(null);
        }

        foreach (var kvp in items)
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;

            
            // Find the corresponding slot for the item
            Slot targetSlot = FindSlotForItem(item);
            
            if (targetSlot != null)
            {
                GD.Print($"Updating slot {itemEnum} with {item.quantity} items. itemIndex: {targetSlot.GetIndex()}");
                targetSlot.update(item);
                targetSlot.SetCurrentSlotItem(item);
            }
        }
    }

    private bool grabbed = false;
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("close_inv"))
            Visible = false;

        
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
                    moveSprite();
                }

                if (Input.IsActionJustReleased("left_mouse_click"))
                {
                    grabbed = false;
                    swapItems();
                    SetGrabbedState(false);
                    moveSprite();
                    UpdateSlots();
                }
            }
        }
    }

    public void SetGrabbedState(bool state)
    {
        foreach (var slot in slots)
        {
            slot.SetGrabbedState(state);
        }
    }
    
    private Slot FindSlotForItem(Item item)
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
            heldItemSprite.Position = GetGlobalMousePosition() - mouseOffset;
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
        gridContainer.MoveChild(grabbedItem, targetIndex);
        gridContainer.MoveChild(hoverOverItem, grabbedIndex);
    }
    
    public void SetVisibility(bool state)
    {
        Visible = state;
    }
}