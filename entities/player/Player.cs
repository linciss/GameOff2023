using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    private float Speed = 5.0f;
    [Export]
    private float JumpVelocity = 4.5f;

    [Export]
    private Camera3D camera;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        camera = GetNode<Camera3D>("Camera3D");
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
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity.Y -= gravity * (float)delta;
            GD.Print(velocity);
        }
        else
        {
            velocity.Y = 0;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        Velocity = velocity;

        MoveAndSlide();
    }
}
