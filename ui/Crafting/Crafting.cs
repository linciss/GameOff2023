using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GameOff2023.globals.Inventory;
using Godot;

public partial class Crafting : Control, IItemHolder
{
    public static Dictionary<ItemEnum, Item> allCraftables = new Dictionary<ItemEnum, Item>();
    public static Dictionary<ItemEnum, Item> availableCraftables = new Dictionary<ItemEnum, Item>();
    private Player player;
    private IInventory playerInventory;
    private Slot clickedItem;
    [Export]private GridContainer playerGrid;
    [Export]private GridContainer craftingGrid;
    private Slot[] craftingSlots;
    private Slot[] playerSlots;
    private int slotIndex;
     public override void _Ready()
     {
	     player = player_instance.GetPlayer();
	     playerInventory = player.inventory;
	     GD.Print("Crafting ready");
	     if (playerGrid != null)
	     {
		     playerSlots= new Slot[playerGrid.GetChildCount()];
			
		     for (int i = 0; i < playerGrid.GetChildCount(); i++)
		     {
			     Node childNode = playerGrid.GetChild(i);
			     playerSlots[i] = childNode as Slot;
		     }
	     }
	     if (craftingGrid != null)
	     {
		     craftingSlots = new Slot[craftingGrid.GetChildCount()];
			
		     for (int i = 0; i < craftingGrid.GetChildCount(); i++)
		     {
			     Node childNode = craftingGrid.GetChild(i);
			     craftingSlots[i] = childNode as Slot;
			     craftingSlots[i].setLabel("");
		     }
	     }
	     InitializeCraftables();
	     UpdatePlayerSlots();
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
    
    public void InitializeCraftables()
    {
	    updateCraftingSlots();
    }
    
    bool craftableAvailable = true;
    public  void CheckCraftable()
    {
	    int iteration = 0;
	    foreach (var item in allCraftables)
	    {
		    int currentCraftableCount = calculateCraftableCount(item.Value);
		    if (currentCraftableCount > 0)
		    {
			    availableCraftables[item.Key] = item.Value;
		    }
		    else
		    {
			    availableCraftables.Remove(item.Key);
		    }
	    }
    }

    public void updateCraftingSlots()
    {
	    int iteration = 0;
	    foreach (var item in ItemFactory.allItems)
	    {
		    if (item.Value.recipe != null)
		    {
			    allCraftables[item.Key] = item.Value;
			    craftingSlots[iteration].update(item.Value);
			    
			    int currentCraftableCount = calculateCraftableCount(item.Value);
			    if (currentCraftableCount > 0)
			    {
				    availableCraftables[item.Key] = item.Value;
			    }
			    craftingSlots[iteration].setLabel(currentCraftableCount + "");
			    iteration++;
		    }
	    }
    }
    
    //responsible for crafting items
    public void CraftItem(ItemEnum itemEnum)
    {
	    if (craftableAvailable)
	    {
		    foreach (var recipe in availableCraftables[itemEnum].recipe)
		    {
			    Enum.TryParse(recipe.item, out ItemEnum itemEnumRecipe);
			    player.inventory.RemoveItem(itemEnumRecipe, recipe.quantity);
		    }
		    player.inventory.AddItem(itemEnum, 1);
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
    
    public override void _Process(double delta)
    {
	    if (Input.IsActionJustPressed("left_mouse_click") && clickedItem != null)
	    {
		    CheckCraftable();
		    if (clickedItem.getItem() != null)
		    {
			    if (clickedItem.getItem().recipe != null && availableCraftables.ContainsKey(clickedItem.getItem().type))
			    {
				    CraftItem(clickedItem.getItem().type);
				    UpdatePlayerSlots();
				    updateCraftingSlots();
			    }
		    }
	    }
    }
    
    public void SetGrabbedItem(Slot slot)
    {
	    clickedItem = slot;
    }

    public void SetHoveredItem(Slot slot){}


    public void SetGrabbedState(bool state){}

	
    public void SetSlotIndex(int index)
    {
	    slotIndex = index;
    }
    
    
    public void SetCurrentSlotIndex(int index){}
    
    public Slot GetGrabbedItem()
    {
	    return clickedItem;
    }
    
    public int calculateCraftableCount(Item item)
    {
	    int craftableCount = int.MaxValue;

	    foreach (var recipe in item.recipe)
	    {
		    Enum.TryParse(recipe.item, out ItemEnum itemEnum);

		    if (!player.inventory.inventory.ContainsKey(itemEnum))
		    {
			    craftableCount = 0;
			    break;
		    }
		    else
		    {
			    int requiredQuantity = recipe.quantity;
			    int availableQuantity = player.inventory.inventory[itemEnum].quantity;
			    int possibleCrafts = availableQuantity / requiredQuantity;
			    craftableCount = Math.Min(craftableCount, possibleCrafts);
		    }
	    }

	    return craftableCount;
    }

}