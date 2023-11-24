using Godot;
using System;
using GameOff2023.entities.placeable.test;

public partial class GridHover : Node3D
{
    [Export]
    private StaticBody3D hoverCollider;

    [Export]
    private ProgressBar progressBar;
    [Export]
    private Sprite3D progressBarSprite;


    [Export]
    private float mineTime = 1.0f;

    [Export]
    private bool placeMode = false;

    Player player;
    private static GridHover Instance { get; set; }  
    public static GridHover getInstance()
    {
        return Instance;
    }
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Instance = this;
        player = Player.GetPlayer();
        progressBar.MaxValue = mineTime;
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
            
            //Sets hover objects postion
            Position = GridSystem.translateToRelativePos(targetPos);

            if (placeMode)
            {
                handleHoverPlace(targetPos, delta);
            }else if (GridSystem.getCell((Vector3I)targetPos) != null)
            {
                handleHover(GridSystem.getCell((Vector3I)targetPos), delta);
            }else
            {
                isHeld = false;
                holdTime = 0.0f;
                progressBar.Visible = false;
            }
        }
    }


    bool isHeld = false;
    float holdTime = 0.0f;
    Cell lastCell = null; 
    public void handleHover(Cell cell, double delta)
    {
        //Checks if the cell is changed
        if (cell != lastCell)
        {
            holdTime = 0.0f;
            lastCell = cell;
        }
        
        //Detect if left click
        if (Input.IsActionPressed("left_mouse_click") && !placeMode)
        {

            isHeld = true;
            holdTime += (float)delta;
            progressBar.Value = holdTime / mineTime;

            progressBar.Visible = true;

            if (holdTime >= mineTime)
            {
                mineItem(cell);
            }
        }
        else
        {
            isHeld = false;
            holdTime = 0.0f;
            progressBar.Visible = false;
        }
    }

    public void mineItem(Cell cell)
    {
        switch (cell.item)
        {
            case ItemEnum.RawSteel:
                player.inventoryAPI.AddItem(ItemEnum.RawSteel, 1);
                break;
            case ItemEnum.RawCopper:
                player.inventoryAPI.AddItem(ItemEnum.RawCopper, 1);
                break;
        }
        
        holdTime = 0.0f;
        player.inventoryAPI.PrintAllItems();
    }

    public void handleHoverPlace(Vector3 pos, double delta)
    {
        if (Input.IsActionJustPressed("left_mouse_click"))
        {
            GridSystem.setPosition(pos, new BeltMachine(GetTree()));
        }
    }
    
    public void setPlaceMode(bool placeMode)
    {
        this.placeMode = placeMode;
    }
    
    public bool getPlaceMode()
    {
        return placeMode;
    }
}
