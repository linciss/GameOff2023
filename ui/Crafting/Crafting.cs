using System;
using System.Collections.Generic;
using Godot;

public partial class Crafting : Node
{
    public static Dictionary<ItemEnum, Item> allCraftables = new Dictionary<ItemEnum, Item>();
    public static Dictionary<ItemEnum, Item> availableCraftables = new Dictionary<ItemEnum, Item>();

    static Crafting()
    {
        InitializeCraftables();
    }
    
    public static void InitializeCraftables()
    {
        foreach (var item in ItemFactory.allItems)
        {
            if (item.Value.recipe != null)
            {
                allCraftables[item.Key] = item.Value;
                GD.Print($"CALLED FROM CRAFTING CLASS!");
            }
        }
    }

    //this is for the gui shit
    static bool craftableAvailable = true;
    public static void CheckCraftable()
    {
        foreach (var item in allCraftables)
        {
            
            foreach (var recipe in item.Value.recipe)
            {
                Enum.TryParse(recipe.item, out ItemEnum itemEnum);
                
                if (!InventoryAPI.inventory.ContainsKey(itemEnum))
                {
                    craftableAvailable = false;
                    break;
                }
                else if (InventoryAPI.inventory[itemEnum].quantity < recipe.quantity)
                {
                    craftableAvailable = false;
                    break;
                }
            }
            if (craftableAvailable)
                availableCraftables[item.Key] = item.Value;
            else
                availableCraftables.Remove(item.Key);
        }
    }

    public static void CraftItem(ItemEnum itemEnum)
    {
        if (craftableAvailable)
        {
            foreach (var recipe in availableCraftables[itemEnum].recipe)
            {
                Enum.TryParse(recipe.item, out ItemEnum itemEnumRecipe);
                InventoryAPI.RemoveItem(itemEnumRecipe, recipe.quantity);
            }
            InventoryAPI.AddItem(itemEnum, 1);
        }else
            GD.Print($"Not enough resources to craft {itemEnum}");
    }
}
