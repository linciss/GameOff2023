using Godot;
using System;
using GameOff2023.entities;
using GameOff2023.entities.belt;
using GameOff2023.entities.cell_items;
using GameOff2023.entities.chest;
using GameOff2023.entities.placeable;
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

    private PlaceableType placeableType = PlaceableType.None;

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
        handleRotation();
        
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
            else if (normal.Equals(new Vector3(-1, 0, 0))) normal = Vector3.Zero;
            // else if (normal.IsEqualApprox(new Vector3(0, 1, 0))) normal = new Vector3(0, -1, 0);
            // else if (normal.IsEqualApprox(new Vector3(0, 0, 1))) normal = new Vector3(0, 0, 0);

            //add the normal so it places side of block if aiming at the side of block
            Vector3 targetPos = (Vector3)result["position"] - normal;
            
            //Sets hover objects postion
            Position = GridSystem.translateToRelativePos(targetPos);
            
            if (normal == Vector3.Zero)
            {
                targetNormal = new Vector3(0, 1, 0);
            }
            else
            {
                targetNormal = normal;
            }

            if (placeMode)
            {
                handleHoverPlace(GridSystem.translateToGridPos(Position), delta);
            }else if (GridSystem.getCell(GridSystem.translateToGridPos(Position)).hasCellItem())
            {
                handleHover(GridSystem.getCell(GridSystem.translateToGridPos(Position)), delta);
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
        if (!player.getOpened())
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
            }else if (Input.IsActionJustPressed("open_placeable"))
            {
                if (cell.getCellItem() is ChestMachine)
                {
                    ChestMachine chestMachine = (ChestMachine)cell.getCellItem();
                    if (!player.getOpened())
                    {
                        chestMachine.handleOpen();
                    }
                }
            }
            else
            {
                isHeld = false;
                holdTime = 0.0f;
                progressBar.Visible = false;
            }
        }
       
    }

    public void handleRotation()
    {
        if (Input.IsActionJustPressed("rotate_right"))
        {
            RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y - 90, RotationDegrees.Z);
            if (RotationDegrees.Y <= -360) RotationDegrees = new Vector3(RotationDegrees.X, 0, RotationDegrees.Z);
        }
        else if (Input.IsActionJustPressed("rotate_left"))
        {
            RotationDegrees = new Vector3(RotationDegrees.X, RotationDegrees.Y + 90, RotationDegrees.Z);
            if (RotationDegrees.Y >= 360) RotationDegrees = new Vector3(RotationDegrees.X, 0, RotationDegrees.Z);
        }
        
    }
    
    public void mineItem(Cell cell)
    {
        //Get placeable item from cell
        ICellItem item = cell.getCellItem();
        
        //If not Ore then idi nahuj
        if (!(item is IOre)) return;

        //If scoobi doobi do is Ore
        IOre ore = (IOre)item;
        player.inventory.AddItem(ore.getOreType(), 1);
        
        holdTime = 0.0f;
        player.inventory.PrintAllItems();
    }

    public void handleHoverPlace(Vector3I pos, double delta)
    {
        
        if (Input.IsActionJustPressed("left_mouse_click"))
        {// TODO check if isint filled
            GD.Print("HOVER PLACE POS: " + pos);
            ICellItem toBePlaced=null;
            switch (placeableType)
            {
                case PlaceableType.None:
                    break;
                case PlaceableType.Belt:
                    toBePlaced = new BeltMachine(
                        GridSystem.getCell(GridSystem.translateToGridPos(pos)),
                        RotationDegrees,
                        GetTree()
                    );
                    break;
                case PlaceableType.Chest:
                    toBePlaced = new ChestMachine(
                        GridSystem.getCell(GridSystem.translateToGridPos(pos)),
                        RotationDegrees,
                        GetTree()
                    );
                    break;
            }
            if (toBePlaced == null) return;
            GridSystem.setPosition( pos, toBePlaced );
        }

        if (Input.IsActionJustPressed("middle_mouse_click"))
        {
            Vector3I gridPos = GridSystem.translateToGridPos(pos);
            if (GridSystem.getCell(gridPos).hasCellItem())
            {
                if (GridSystem.getCell(gridPos).getCellItem() is IPlaceable placeable)
                {
                    placeable.getInventory().PrintAllItems();
                }
            }
            
        }
        // else if (Input.IsActionJustPressed("right_mouse_click"))
        // {
        //    //TODO interact
        //    
        // }
    }

    public void setPlaceMode(bool placeMode)
    {
        this.placeMode = placeMode;
    }
    
    public bool getPlaceMode()
    {
        return placeMode;
    }
    
    public void setPlaceableType(PlaceableType placeableType)
    {
        this.placeableType = placeableType;
    }
}
