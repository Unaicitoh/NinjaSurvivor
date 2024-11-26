using Godot;
using System;
using System.Runtime.Serialization;

public partial class Projectile : Area2D
{
	[Export]
	private float speed = 200f;
	private const int offset = 25;
	public Vector2 initialPosition { get; set; }
	public float initialRotation { get; set; }
	public Vector2 velocity { get; set; } = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		// Sets the bullet away from the player
		Position = initialPosition;
		Rotation = initialRotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += velocity * speed * (float)delta;
	}

	public void init(Vector2 position, Vector2 velocity, float rotation)
	{
		initialPosition = position + velocity * offset;
		this.velocity = velocity;
		initialRotation = rotation;
	}

	private void OnCollisionDetected(Node2D body)
	{
		GD.Print("Collisioning");
		velocity = Vector2.Zero;
	}
}
