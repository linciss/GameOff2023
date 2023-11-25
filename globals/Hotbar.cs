using Godot;
using System;
using GameOff2023.entities.placeable;

public partial class Hotbar : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private Button mineButton;
	[Export]
	private Button beltButton;
	[Export]
	private Button chestButton;
	public override void _Ready()
	{
		// mineButton=GetNode<Button>("MineButton");
		// beltButton=GetNode<Button>("BeltButton");
		// mineButton=GetNode<Button>("ChestButton");
		
		mineButton.Connect(Button.SignalName.Pressed, Callable.From(OnMineButtonPressed));
		beltButton.Connect(Button.SignalName.Pressed, Callable.From(OnBeltPlace));
		chestButton.Connect(Button.SignalName.Pressed, Callable.From(OnChestPlace));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnMineButtonPressed()
	{
		GridHover.getInstance().setPlaceMode(false);
		GridHover.getInstance().setPlaceableType(PlaceableType.None);
	}
	
	public void OnBeltPlace()
	{
		GridHover.getInstance().setPlaceMode(true);
		GridHover.getInstance().setPlaceableType(PlaceableType.Belt);
	}
	
	public void OnChestPlace()
	{
		GridHover.getInstance().setPlaceMode(true);
		GridHover.getInstance().setPlaceableType(PlaceableType.Chest);
	}
	
	
}
