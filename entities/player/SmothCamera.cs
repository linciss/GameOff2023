using Godot;
using System;

public partial class SmothCamera : Camera3D
{
	[Export]
	private Player Player = null;


	[Export]
	private float Speed = 5.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Prevent the camera from being glued to the player
        TopLevel = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		LookAtDelayed();
	}

    private async void LookAtDelayed()
    {
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        LookAt(Player.GlobalTransform.Origin, Vector3.Up);
    }
}
