using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float speed = 150.0f;
	private AnimationPlayer animation;
	private Sprite2D weapon;
	private const float rotation_factor = .5f;

	public override void _Ready()
	{
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		weapon = GetNode<Sprite2D>("MeleeWeapon");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Player movement
		Vector2 playerDirection = Input.GetVector("left", "right", "up", "down");
		if (playerDirection != Vector2.Zero)
		{
			velocity = playerDirection * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
			// animation.Play("idle");
			animation.Stop();
		}
		Velocity = velocity;
		MoveAndSlide();

		// Top-Right-Left-Bottom ANIMATION + ROTATION
		/*Need Player Global Position due to World Coordinates (Viewport mouse position would need a conversion)
		 GetViewportTransform() * GlobalPosition */
		var mousePosition = GetGlobalMousePosition();
		var mouseDirection = (mousePosition - GlobalPosition).Normalized();
		var targetRotation = mouseDirection * rotation_factor;
		// GD.Print($"Mouse direction offset from player: {mouseDirection}");
		if (Mathf.Abs(mouseDirection.X) > Mathf.Abs(mouseDirection.Y))
		{
			if (mouseDirection.X > 0)
			{
				Rotation = targetRotation.Y;
				animation.Play("walk_right");
			}
			else //FlipH updates on Animations + Godot counter-clockwise rotations as positive values FORCE this negation
			{
				Rotation = -targetRotation.Y;
				animation.Play("walk_left");
			}
		}
		else
		{
			if (mouseDirection.Y > 0)
			{
				Rotation = -targetRotation.X;
				animation.Play("walk_down");
			}
			else
			{
				Rotation = targetRotation.X;
				animation.Play("walk_up");
			}
		}
	}
}
