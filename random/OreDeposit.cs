using Godot;
using System;

public partial class OreDeposit : Node3D
{
	[Export]
	private ItemEnum item;
	public override void _Ready()
	{	
		GridSystem.setPosition(
			this.Position, 
			new Cell(
                this.Name, 
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
