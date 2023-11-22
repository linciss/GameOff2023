using Godot;
using System;

public partial class OreDeposit : Node3D
{
	[Export]
	private ItemEnum item;
	public override void _Ready()
	{	
		GridSystem.setPosition(
			Position, 
			new Cell(
                Name, 
                this, 
                item
			)
			);
	}

	public ItemEnum getItem()
	{
        return item;
    }
}
