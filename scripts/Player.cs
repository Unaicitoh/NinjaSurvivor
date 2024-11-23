using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float speed = 150.0f;
	private AnimationPlayer animation;
	private Sprite2D weapon;

	public override void _Ready()
	{
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		weapon = GetNode<Sprite2D>("MeleeWeapon");
	}
	public override void _Process(double delta)
	{
		Vector2 velocity = Velocity;
		// Input Map directions
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
			if (direction.X > 0)
			{
				animation.Play("walk_right");
			}
			else if (direction.X < 0)
			{
				animation.Play("walk_left");
			}
			else if (direction.Y > 0)
			{
				animation.Play("walk_down");
			}
			else
			{
				animation.Play("walk_up");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, speed);
			animation.Play("idle");
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
