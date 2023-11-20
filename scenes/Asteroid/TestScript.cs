using Godot;
using System;

public partial class TestScript : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.

	public void _on_area_3d_mouse_entered()
	{
		//GD.Print("Mouse entered");
	}

    public void _on_area_3d_input_event(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left && this.Name == "steel")
            {
                {
                    InventoryAPI.AddItem(ItemEnum.RawSteel, 1);
                    InventoryAPI.PrintAllItems();
                }
            }
            if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left && this.Name == "copper")
            {
                {
                    InventoryAPI.AddItem(ItemEnum.RawCopper, 1);
                    InventoryAPI.PrintAllItems();
                }
            }
            if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left && this.Name == "remove steel")
            {
                {
                    InventoryAPI.RemoveItem(ItemEnum.RawSteel, 1);
                    InventoryAPI.PrintAllItems();
                }
            }
        }
        
    }

        
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
       
 

    }
}
