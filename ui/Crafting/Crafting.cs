using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;

public partial class Crafting : Node
{
    public static Dictionary<ItemEnum, Item> allCraftables = new Dictionary<ItemEnum, Item>();
    public static Dictionary<ItemEnum, Item> availableCraftables = new Dictionary<ItemEnum, Item>();
    private Player player;
    
     public override void _Ready()
     {
	     
	     GD.Print("Crafting ready");
	 }	
    //for now like this because this shit doesnt have a scene
    public  Crafting(Player playerInstance)
    {
	    player =playerInstance;
    	InitializeCraftables();
    } 
    
    public void InitializeCraftables()
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
    bool craftableAvailable = true;
    public  void CheckCraftable()
    {
	    foreach (var item in allCraftables)
	    {
		    foreach (var recipe in item.Value.recipe)
		    {
			    Enum.TryParse(recipe.item, out ItemEnum itemEnum);

			    if (!player.inventoryAPI.inventory.ContainsKey(itemEnum))
			    {
				    craftableAvailable = false;
				    break;
			    }
			    else if (player.inventoryAPI.inventory[itemEnum].quantity < recipe.quantity)
			    {
				    craftableAvailable = false;
				    break;
			    }
			    else
			    {
				    craftableAvailable = true;
			    }
		    }
		    if (craftableAvailable)
			    availableCraftables[item.Key] = item.Value;
		    else
			    availableCraftables.Remove(item.Key);
	    }
    }

    public void CraftItem(ItemEnum itemEnum)
    {
	    if (craftableAvailable)
	    {
		    GD.Print("CRAFTABLE");
		    foreach (var recipe in availableCraftables[itemEnum].recipe)
		    {
			    Enum.TryParse(recipe.item, out ItemEnum itemEnumRecipe);
			    player.inventoryAPI.RemoveItem(itemEnumRecipe, recipe.quantity);
		    }
		    player.inventoryAPI.AddItem(itemEnum, 1);
	    }else
		    GD.Print($"Not enough resources to craft {itemEnum}");
    }
	
}