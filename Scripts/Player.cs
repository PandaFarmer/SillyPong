using Godot;
using System;

public partial class Player : CharacterBody3D //KinematicBody3D on Godot 3
{
    KinematicCollision3D collisionData;
    //fields for animation
    double timeElapsed;
    bool counting = false;

    public override void _Ready()
	{
        base._Ready();
        GetNode<MeshInstance3D>("MeshInstance3D").Visible = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        bool isPlayer1 = "Player1" == GetName();
        bool isPlayer2 = "Player2" == GetName();

        if ((Input.IsKeyPressed(Key.W) && isPlayer1) ||
            (Input.IsKeyPressed(Key.Up) && isPlayer2))
        {
            Vector3 currentPosition = Position;
            currentPosition.X -= .1f;
            Position = currentPosition;
        }
        if ((Input.IsKeyPressed(Key.S) && isPlayer1) ||
            (Input.IsKeyPressed(Key.Down) && isPlayer2))
        {
            Vector3 currentPosition = Position;
            currentPosition.X += .1f;
            Position = currentPosition;
        }

        if(counting)
        {
            timeElapsed += delta;
        }
        MoveAndSlide();

        // Check for collisions after movement
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            // if (collision.GetCollider().GetMeta("Owner").ToString() != "")
            string colliderMetaName = collision.GetCollider().GetMeta("Name").AsString();

            //handle Ball collision
            if (colliderMetaName == "Ball")
            {
                GetNode<MeshInstance3D>("MeshInstance3D").Visible = true;
                GetNode<Node3D>("Orc").Visible = false;
                timeElapsed = 0f;
                counting = true;
            }
            // GD.Print($"Collided with: {collision.GetCollider().GetMeta("Name").AsString()}");
        }
        
        if(timeElapsed >= .2f)
        {
            GetNode<MeshInstance3D>("MeshInstance3D").Visible = false;
            GetNode<Node3D>("Orc").Visible = true;
            counting = false;
        }
    }
}