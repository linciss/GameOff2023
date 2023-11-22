using Godot;
using System;

public partial class Hotbar : HBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public Button mineButton;
	public Button testPlace1;
	public override void _Ready()
	{
		mineButton=GetNode<Button>("MineButton");
		testPlace1=GetNode<Button>("TestPlace1");
		
		mineButton.Connect(Button.SignalName.Pressed, Callable.From(OnMineButtonPressed));
		testPlace1.Connect(Button.SignalName.Pressed, Callable.From(OnTestPlace1));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnMineButtonPressed()
	{
		GridHover.getInstance().setPlaceMode(false);
	}
	
	public void OnTestPlace1()
	{
		GridHover.getInstance().setPlaceMode(true);
	}
	
	
}
