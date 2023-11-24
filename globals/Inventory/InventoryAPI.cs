using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryAPI : Node, IInventory
{
    public Dictionary<ItemEnum, Item> inventory = new Dictionary<ItemEnum, Item>();

    public event Action InventoryChanged;

    public void AddItem(ItemEnum itemEnum, int quantity)
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
            inventory[itemEnum].RemoveQuantity(quantity);

            if (inventory[itemEnum].quantity == 0)
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
}