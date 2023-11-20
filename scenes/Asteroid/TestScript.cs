using Godot;
using System;

public partial class TestScript : MeshInstance3D
{

    private bool isHeld = false;
    private float holdTime = 0.0f;
    private float mineTime = 2.0f;


    public void _on_area_3d_input_event(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shape_idx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseButtonEvent.Pressed)
                {
                    isHeld = true;
                }else if (mouseButtonEvent.IsReleased())
                {
                    isHeld = false;
                    holdTime = 0.0f;
                }
            }
        }

    }

       
	public override void _Process(double delta)
	{

        if (isHeld)
        {
            holdTime += (float)delta;

            if (holdTime >= mineTime) {
                if (this.Name == "steel")
                {
                    InventoryAPI.AddItem(ItemEnum.RawSteel, 1);
                    InventoryAPI.PrintAllItems();
                    holdTime = 0.0f;
                }
                else if (this.Name == "copper")
                {
                    InventoryAPI.AddItem(ItemEnum.RawCopper, 1);
                    InventoryAPI.PrintAllItems();
                    holdTime = 0.0f;
                }
            }
           
        }

    }
}
