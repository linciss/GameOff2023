using Godot;
using System;
using System.Collections.Generic;
using GameOff2023.entities;
using GameOff2023.entities.chest;
using GameOff2023.entities.placeable;

public partial class inv_ui : Control
{
    public bool open = false;
    [Export]
    public Slot[] slots;
    private ICellItem cellItem;
    [Export] private Label test;
    [Export]
    private GridContainer gridContainer;

    public string name = "lfuck off";
    IInventory inventory;

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
            UpdateSlots();
        }
    }

    public void UpdateSlots()
    {
        int iteration = 0;
        GD.Print("UPDATE SLOTS");

        Dictionary<ItemEnum, Item> items = inventory.GetInventory();

        // Wipe invUI
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].update(null);
        }

        foreach (var kvp in items)
        {
            ItemEnum itemEnum = kvp.Key;
            Item item = kvp.Value;

            GD.Print($"Updating slot {itemEnum} with {item.quantity} items. ITERATION {iteration} item: {item}");
            slots[iteration].update(item);
            iteration++;
        }
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("close_inv"))
            Visible = false;
    }
}