using Godot;
using System;

public partial class Ball : CharacterBody3D //KinematicBody2D for Godot 3
{
	//use the following line on Godot 3
	//Vector2 Velocity;
	[Signal]
	public delegate void OutOfBoundsLeftEventHandler();
	[Signal]
	public delegate void OutOfBoundsRightEventHandler();

	KinematicCollision3D collisionData;

	//fields for animation
	double timeElapsed;
	bool counting = false;

	public override void _Ready()
	{
		base._Ready();
		reset();
	}

	private void reset()
	{
		RandomNumberGenerator r = new RandomNumberGenerator();
		Velocity = new Vector3(r.RandfRange(-1, 1), 0, 2f);
		Position = Vector3.Zero;
		GetNode<Node3D>("Ahegao").Visible = false;
		counting = false;
		timeElapsed = 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(counting)
		{
			timeElapsed += delta;
		}

		collisionData = MoveAndCollide(Velocity * new Vector3((float)delta, (float)delta, (float)delta));

		if (collisionData != null)
		{
			//randomize the y value a little
			RandomNumberGenerator r = new RandomNumberGenerator();
			Vector3 tempVelocity = Velocity;
			tempVelocity.Y += r.RandfRange(-20, 20);
			Velocity = tempVelocity;

			//bounce
			Velocity = Velocity.Bounce(collisionData.GetNormal());

			string colliderMetaName = collisionData.GetCollider().GetMeta("Name").AsString();
		
			GD.Print($"Collided with: {collisionData.GetCollider().GetMeta("Name").AsString()}");
			//handle Ball collision
			if (colliderMetaName.Contains("Player"))
			{
				GetNode<Node3D>("Ahegao").Visible = true;
				GetNode<Node3D>("AngryElf").Visible = false;
				timeElapsed = 0f;
				counting = true;
			}
		}

		if(timeElapsed >= 1f)
		{
			GetNode<Node3D>("Ahegao").Visible = false;
			GetNode<Node3D>("AngryElf").Visible = true;
			counting = false;
		}

		CheckBounds();
	}

	private void CheckBounds()
	{
		if(Position.Z > 3f)
		{
			EmitSignal(SignalName.OutOfBoundsRight);
			reset();
		}
		if(Position.Z < -3f)
		{
			EmitSignal(SignalName.OutOfBoundsLeft);
			reset();
		}
	}
}
