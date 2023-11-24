using Godot;
using System;
using GameOff2023.entities.cell_items.steel_ore;
using GameOff2023.entities.placeable;

public partial class OreDeposit : Node3D
{
	[Export]
	private ItemEnum item;
	public override void _Ready()
	{
		ICellItem ore;
		Cell targetCell=GridSystem.getCell(GridSystem.translateToGridPos(Position));
		switch (item)
		{
			case ItemEnum.RawSteel:
				ore = new SteelOre(this, targetCell);
				break;
			case ItemEnum.RawCopper:
				ore = new CopperOre(this, targetCell);
				break;
			default:
				GD.Print("wrong ore type");
				return;
		}
		
		GridSystem.setPosition(
			Position, 
				ore
			);
	}

	public ItemEnum getItem()
	{
        return item;
    }
}
