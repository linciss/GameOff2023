﻿using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryAPI : Node, IInventory
{
    public Dictionary<ItemEnum, Item> inventory = new Dictionary<ItemEnum, Item>();

    public event Action InventoryChanged;
    
    private int maxSlots = 20;
    
    public InventoryAPI(){}
    
    public InventoryAPI(int maxSlots)
    {
        this.maxSlots = maxSlots;
    }
    public void AddItem(ItemEnum itemEnum, int quantity) //TODO check if item is full if so add to next slot
    {
        if (inventory.ContainsKey(itemEnum))
        {
            inventory[itemEnum].AddQuantity(quantity);
        }
        else
        {
            Item item = ItemFactory.newItem(itemEnum);

            GD.Print("Adding " + item.name + " through item factory!!!!!");

            item.quantity = quantity;

            inventory[itemEnum] = item;
        }
        NotifyInventoryChanged();
    }

    public void RemoveItem(ItemEnum itemEnum, int quantity)
    {
        if (inventory.ContainsKey(itemEnum))
        {
            if (!inventory[itemEnum].RemoveQuantity(quantity))
            {
                inventory.Remove(itemEnum);
            }
        }
        else
        {
            GD.Print("Item not found in inventory");
        }

        NotifyInventoryChanged();
    }

    public Dictionary<ItemEnum, Item> GetInventory()
    {
        return inventory;
    }

    public void PrintAllItems()
    {
        GD.Print("IThis is my inventory");
        foreach (var kvp in inventory)
        {
            GD.Print($"Name: {kvp.Value.name}, Quantity: {kvp.Value.quantity}, Image: {kvp.Value.image}");
        }
    }

    private void NotifyInventoryChanged()
    {
        InventoryChanged?.Invoke();
    }
    
    public bool CanAddItem(ItemEnum itemEnum, int quantity)
    {
        if (inventory.ContainsKey(itemEnum)) 
        {
            if (inventory[itemEnum].quantity + quantity > ItemFactory.GetItemMaxStackSize(itemEnum)) 
            {
                return false;
            }
        }
        if (inventory.Count + 1 > maxSlots) 
        {
            return false;
        }

        return true;
    }
    public Item GetItemAtIndex(int index)
    {
        var items = new List<Item>(inventory.Values);
        
        if (index >= 0 && index < items.Count)
        {
            return items[index];
        }

        return null;
    }

}