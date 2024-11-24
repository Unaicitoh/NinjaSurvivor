using Godot;
using System;

public partial class Game : Node2D
{
	private CharacterBody2D player;
	private Marker2D playerSpawn;

	public override void _Ready()
	{
		player = GetNode<CharacterBody2D>("Player");
		playerSpawn = GetNode<Marker2D>("Arena/Marker2D");
		player.Position = playerSpawn.Position;
	}

	public override void _Process(double delta)
	{

	}
}
