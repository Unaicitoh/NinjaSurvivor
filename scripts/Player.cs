using Enums;
using Godot;
using System;
using static Enums.Direction;

public partial class Player : CharacterBody2D
{
	[Export]
	private float speed = 100f;
	private const float rotation_factor = .4f;
	private AnimationPlayer animation;
	private PackedScene projectileScene;
	private Area2D meleeWeapon;
	private Sprite2D rangedWeapon;
	private bool isAttacking = false;
	private Vector2 mouseDirection;

	public override void _Ready()
	{
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		meleeWeapon = GetNode<Area2D>("Area2D");
		rangedWeapon = GetNode<Sprite2D>("RangeWeapon");
		projectileScene = GD.Load<PackedScene>("res://scenes/Projectile.tscn");
	}
	public override void _PhysicsProcess(double delta)
	{
		/*Need Player Global Position due to World Coordinates (Viewport mouse position would need a conversion)
		 GetViewportTransform() * GlobalPosition */
		mouseDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		// GD.Print($"Mouse direction offset from player: {mouseDirection}");

		// Player movement logic
		Vector2 velocity = Velocity;
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
			if (!isAttacking)
			{
				animation.Stop();
			}
		}

		if (isAttacking)
		{
			velocity /= 2;
		}
		else
		{
			playAnimation("walk");
		}
		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("melee_attack") && !isAttacking)
		{
			swapWeaponVisibility(true);

			isAttacking = true;
			playAnimation("attack");
		}
		else if (@event.IsActionPressed("ranged_attack") && !isAttacking)
		{
			swapWeaponVisibility(false);
			var projectile = (Projectile)projectileScene.Instantiate();
			projectile.init(GlobalPosition, mouseDirection, mouseDirection.Angle());
			isAttacking = true;
			playAnimation("attack");
			GetTree().CurrentScene.AddChild(projectile);
		}
	}

	private void OnAnimationFinished(StringName animationName)
	{
		if (animationName.ToString().StartsWith("attack"))
		{
			isAttacking = false;
		}
		if (Input.IsActionPressed("melee_attack"))
		{
			isAttacking = true;

			playAnimation("attack");
		}
	}

	//Checks the mouse position relative to the player and return the equivalent Quadrant
	private Direction getMouseQuadrant()
	{
		if (Math.Abs(mouseDirection.X) > Math.Abs(mouseDirection.Y))
		{
			if (mouseDirection.X > 0)
			{
				return RIGHT;
			}
			else
			{
				return LEFT;
			}
		}
		else
		{
			if (mouseDirection.Y > 0)
			{
				return DOWN;
			}
			else
			{
				return TOP;
			}
		}
	}

	// Top-Right-Left-Bottom Movement/Attack Animation + ROTATION
	// Need to negate some directions due to flipH property on Animations
	private void playAnimation(string animationType)
	{
		var targetRotation = mouseDirection * rotation_factor;
		switch (getMouseQuadrant())
		{
			case TOP:
				Rotation = targetRotation.X;
				animation.Play($"{animationType}_up");
				break;
			case RIGHT:
				Rotation = targetRotation.Y;
				animation.Play($"{animationType}_right");
				break;
			case DOWN:
				Rotation = -targetRotation.X;
				animation.Play($"{animationType}_down");
				break;
			case LEFT:
				Rotation = -targetRotation.Y;
				animation.Play($"{animationType}_left");
				break;
			default:
				GD.PrintErr("Invalid Direction to Move");
				break;
		}
	}

	private void swapWeaponVisibility(bool visibility)
	{
		meleeWeapon.Monitoring = visibility;
		meleeWeapon.Visible = visibility;
		rangedWeapon.Visible = !visibility;
	}
}
