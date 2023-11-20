using Godot;
using System;

public partial class GridHover : Node3D
{

    [Export]
    private Node3D hoverNode;
    [Export]
    private StaticBody3D hoverCollider;

    [Export]
    private bool placeMode = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GD.Print(hoverCollider);
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    Vector3 targetNormal= new Vector3(0,1,0);

    public override void _PhysicsProcess(double delta)
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();

        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 rayFrom = camera.ProjectRayOrigin(mousePosition);
        Vector3 rayTo = rayFrom + camera.ProjectRayNormal(mousePosition) * 1000;

        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Godot.Collections.Dictionary result = spaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(rayFrom, rayTo));

        if (result.Count > 0)
        {
            //if colides with the hover / did disable on the object but just in case
            if ((Node3D)result["collider"] == hoverCollider) return;

            Vector3 normal = (Vector3)result["normal"];

            // Translates normals so can use them to place nicer
            if (normal.IsEqualApprox(new Vector3(0, 1, 0)) && placeMode) normal = Vector3.Zero;
            else if (normal.IsEqualApprox(new Vector3(0, 1, 0))) normal = new Vector3(0, -1, 0);
            else if (normal.IsEqualApprox(new Vector3(0, 0, 1))) normal = new Vector3(0, 0, 0);

            //add the normal so it places side of block if aiming at the side of block
            Vector3 targetPos = (Vector3)result["position"] + normal;

            GridSystem.setPosition(targetPos, this);
        }
    }
}
