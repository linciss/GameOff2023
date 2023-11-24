using System;
using System.Collections.Generic;

using Godot;

public partial class inv_ui : Control
	
{
	bool open = false;
	[Export]
	public  Slot[] slots;
	
	[Export] private GridContainer gridContainer;
	
	Player player;
	
	public override void _Ready()
	{
		player = Player.GetPlayer();
		GD.Print("Wakey wakey eggs and bakey!");
		
		if (gridContainer != null)
		{
			slots = new Slot[gridContainer.GetChildCount()];
			
			for (int i = 0; i < gridContainer.GetChildCount(); i++)
			{
				Node childNode = gridContainer.GetChild(i);
				slots[i] = childNode as Slot;
			}
		}
		else
		{
			GD.Print("GridContainer not found.");
		}
		
		updateSlots();
	}
	
	public void updateSlots()
	{
		int iteration = 0;
		
		//wipes invUI
		for(int i =0 ; i < slots.Length; i++)
		{
			slots[i].update(null);
		}
		
		foreach (var kvp in player.inventoryAPI.inventory)
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
		if (Input.IsActionJustPressed("open_inv"))
		{
			open = !open;
		}
		if(open)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
