﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

public partial class ItemFactory : Node
{

    public static Dictionary<ItemEnum, Item> allItems = new Dictionary<ItemEnum, Item>();

    static ItemFactory()
    {
        InitializeItems();
    }

    public static void InitializeItems()
    {
        string jsonFilePath = "res://globals/Inventory/ItemJson.json";
 
        string jsonContent = FileAccess.GetFileAsString(jsonFilePath);

        List<Item> items = JsonSerializer.Deserialize<List<Item>>(jsonContent);

        foreach (var item in items)
        {
            if (Enum.TryParse<ItemEnum>(item.name, out ItemEnum itemEnum))
            {
                allItems[itemEnum] = item;
            }
            else
            {
                GD.Print($"Bitch not working {item.name}");
            }
        }
    }

    public static int GetItemMaxStackSize(ItemEnum itemEnum)
    {
        return allItems[itemEnum].maxQuantity;
    }
    
    public static void PrintAllItems()
    {
        foreach (var kvp in allItems)
        {
            GD.Print($"Name: {kvp.Value.name}, Quantity: {kvp.Value.quantity}, Image: {kvp.Value.image}");
            if (kvp.Value.recipe != null)
            {
                GD.Print("XD");
            }
        }
    }

    public static Item newItem(ItemEnum itemEnum)
    {
        return allItems[itemEnum].Clone();
    }
}


