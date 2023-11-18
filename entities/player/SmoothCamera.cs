using Godot;
using System;

public partial class SmoothCamera : Camera3D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	private float smoothSpeed = 0.3f;
	[Export]
	private float offset = 0.12f;

	private Player player;
	public override void _Ready()
	{
		player = GetParent<Player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null)
		{
			Vector3 desiredPosition = player.GlobalTransform.Origin + new Vector3(0, offset, 0);
            Vector3 smoothedPosition = GlobalTransform.Origin.Lerp(desiredPosition, smoothSpeed);
            GlobalTransform = new Transform3D(Basis.Identity, smoothedPosition);
		}
	}
}
