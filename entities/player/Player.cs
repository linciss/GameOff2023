using Godot;
using System;
using System.Collections.Generic;
using GameOff2023.entities.chest;

public partial class Player : CharacterBody3D
{
    public static Player Instance { get; private set; }
    
    [Export]
    private float Speed = 5.0f;
    [Export]
    private float JumpVelocity = 4.5f;

    [Export]
    private Camera3D camera;
    [Export]
    private MeshInstance3D mesh;

    public InventoryAPI inventory = new InventoryAPI();
    private Crafting crafting;

    public inv_ui invUis;
    private Control invInstance;
    // [Export]
    // private inv_ui invUi;

    
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public override void _Ready()
    {
        
        Instance = this;
        mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        inventory.AddItem(ItemEnum.RawSteel, 10);
        inventory.AddItem(ItemEnum.RawCopper, 10);
        crafting = new Crafting(Instance);
        
        
        PackedScene invScene = GD.Load<PackedScene>("res://ui/HUD/player_ui.tscn");
        if (invScene != null)
        {
            invInstance = (Control)invScene.Instantiate();
            invUis = invInstance as inv_ui;
            AddChild(invInstance);
            
            inv_ui invUi = invInstance as inv_ui;
        
            if (invUi != null)
            {
                invUi.SetInventory(inventory, "Player123");
            }
        }
        
        // Print items to check their status
        // ItemFactory.PrintAllItems();
        // inventoryAPI.PrintAllItems();
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        Transform3D camXform = camera.GlobalTransform;
        Vector3 cameraForward = -camXform.Basis.Z; // Assuming camera's forward axis is -z.
        Vector3 cameraRight = camXform.Basis.X;    // Assuming camera's right axis is x.

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_down", "ui_up");
        Vector3 input = cameraForward * inputDir.Y + cameraRight * inputDir.X;
        input.Y = 0; // Disregard vertical movement.  

        if (input.LengthSquared() > 0)
        {
            input = input.Normalized() * Speed;
            velocity.X = input.X;
            velocity.Z = input.Z;
            float angle = Mathf.Atan2(input.X, input.Z);
            mesh.Rotation = new Vector3(0, angle, 0);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed); velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }
        

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y -= gravity*2 * (float)delta;
        }
        else
        {
            velocity.Y = 0;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
            crafting.CheckCraftable();
            crafting.CraftItem(ItemEnum.CircuitBoard);
            inventory.PrintAllItems();
        }
           
        

        Velocity = velocity;
        MoveAndSlide();
        handleOpen();
    }
    public static Player GetPlayer()
    {
        return Instance;
    }
    
    private bool opened = false;
    
    public void handleOpen()
    {
        if (Input.IsActionJustPressed("open_inv") && !opened)
        {
            opened = !opened;
            invUis.Position = new Vector2(576, 324);
            invUis.Visible = !invUis.Visible;
        }
        if(Input.IsActionJustPressed("close_inv"))
            opened = false;
    }
    public void openAdjacent()
    {
        invUis.Position = new Vector2(448, 324);
        invUis.Visible = !invUis.Visible;
        opened = !opened;
    }
    
    public bool getOpened()
    {
        return opened;
    }
}
