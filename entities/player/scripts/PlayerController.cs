using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    // Called when the node enters the scene tree for the first time.
    private Vector3 velocity = new Vector3();
    private float speed = 5.0f;
    private float gravity = -9.8f;
    private float jumpHeight = 5.0f;
    private bool isJumping = false;

    public override void _PhysicsProcess(double delta)
    {
        velocity = new Vector3();

        if (Input.IsActionPressed("ui_right"))
            velocity.X += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.Z += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.Z -= 1;

        MoveAndSlide(velocity.Normalized() * speed, Vector3.Up);

    }
   
    private void ProcessInput()
    {
        // Player movement
     

    }

    private void ProcessMovement()
    {

        // Move the player
       

        // If you want to rotate the player based on the movement direction
        if (velocity.LengthSquared() > 0.01)
        {
            Rotation = new Vector3(0, Mathf.Atan2(velocity.X, velocity.Z), 0);
        }
    }
}
