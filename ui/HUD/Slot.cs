using Godot;
using System;

public partial class Slot : Panel
{
	[Export]
	private Sprite2D sprite;
	[Export]
	private Godot.Label label;
	public void update(Item item)
	{
		if (item == null)
		{
			label.Text = "";
			sprite.Texture = null;
			sprite.Visible = false;
		}
		else
		{
			Texture2D texture = GD.Load<Texture2D>(item.image);
			GD.Print($"{item.quantity}");
			label.Text = item.quantity.ToString();
			label.Visible = true;
			float desiredWidth = 17.0f;
			float desiredHeight = 17.0f;
			float originalWidth = texture.GetWidth();
			float originalHeight = texture.GetHeight();
			
			sprite.Texture = texture;
			sprite.Scale = new Vector2(desiredWidth / originalWidth, desiredHeight / originalHeight);
			sprite.Visible = true;
			
		
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
