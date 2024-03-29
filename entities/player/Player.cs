using Godot;
using System;
using System.Collections.Generic;
using GameOff2023.entities.chest;
using GameOff2023.ui.Forge;

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
    private Crafting craftInstance;
    private Smelting smeltingInstance;
    
    // [Export]
    // private inv_ui invUi;

    
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public override void _Ready()
    {
       
        Instance = this;
        player_instance.setPlayer(this);
        mesh = GetNode<MeshInstance3D>("MeshInstance3D");
        inventory.AddItem(ItemEnum.RawSteel, 199);
        inventory.AddItem(ItemEnum.RawCopper, 199);
        PackedScene invScene = GD.Load<PackedScene>("res://ui/HUD/player_ui.tscn");
        if (invScene != null)
        {
            invInstance = (Control)invScene.Instantiate();
            invUis = invInstance as inv_ui;
            AddChild(invInstance);
            
            inv_ui invUi = invInstance as inv_ui;
        
            if (invUi != null)
            {
                invUi.SetInventory(inventory, "Player");
            }
        }
        
        // Print items to check their status
        //ItemFactory.PrintAllItems();
        inventory.PrintAllItems();
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
    private bool craftingOpen = false;
    public void handleOpen()
    {
        if (Input.IsActionJustPressed("open_inv") && !opened)
        {
            opened = !opened;
            invInstance.Position = new Vector2(576, 324);
            invInstance.Visible = !invInstance.Visible;
        }
        if(Input.IsActionJustPressed("close_inv"))
            opened = false;
        
        if (Input.IsActionJustPressed("open_craft") && !craftingOpen)
        {
            if (craftInstance == null)
            {
                PackedScene craftingScene = GD.Load<PackedScene>("res://ui/HUD/CraftingScene.tscn");
                if (craftingScene != null)
                {
                    craftInstance = (Crafting)craftingScene.Instantiate();
                    AddChild(craftInstance);
                    craftInstance.Visible = true;
                    craftingOpen = true;
                }
            }
        }else if (Input.IsActionJustPressed("open_craft") && craftingOpen)
        {
            if(craftInstance != null && IsInstanceValid(craftInstance))
            {
                craftInstance.Visible = false;
                RemoveChild(craftInstance);
                craftInstance = null;
                craftingOpen = false;
            }
        }

        if (Input.IsActionJustPressed("open_forge") && !opened)
        {
            if (smeltingInstance == null)
            {
                PackedScene smeltingScene = GD.Load<PackedScene>("res://ui/HUD/forge.tscn");
                if (smeltingScene != null)
                {
                    smeltingInstance = (Smelting)smeltingScene.Instantiate();
                    AddChild(smeltingInstance);
                    smeltingInstance.Visible = true;
                    opened = true;
                }
            }
        }else if(Input.IsActionJustPressed("open_forge") && opened)
        {
            if(smeltingInstance != null && IsInstanceValid(smeltingInstance))
            {
                smeltingInstance.Visible = false;
                RemoveChild(smeltingInstance);
                smeltingInstance = null;
                opened = false;
            }
        }
    }
    
    public bool getOpened()
    {
        return opened;
    }
}
