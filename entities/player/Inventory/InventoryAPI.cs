using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryAPI :  Node
{
    public static Dictionary<ItemEnum, Item> inventory = new Dictionary<ItemEnum, Item>();

    public static void AddItem(ItemEnum itemEnum, int quantity)
    {
        if (inventory.ContainsKey(itemEnum))
        {
            inventory[itemEnum].AddQuantity(quantity);
        }
        else
        {
            Item item = ItemFactory.newItem(itemEnum);

            GD.Print("Adding " + item.name + "through item factory!!!!!");

            item.quantity = quantity;

            inventory[itemEnum] = item;
        }
    }

    public static void PrintAllItems()
    {
        GD.Print("INVENTORY!!!");
        foreach (var kvp in inventory)
        {
            GD.Print($"Name: {kvp.Value.name}, Quantity: {kvp.Value.quantity}, Image: {kvp.Value.image}");
        }
    }


}

