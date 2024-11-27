using Godot;
using System;

[GlobalClass]
public partial class Weapon : Resource
{
    [Export]
    private string name { get; set; }
    [Export]
    private int damage { get; set; }
    [Export]
    private Texture2D image { get; set; }
    [Export]
    private bool isRanged { get; set; }
    [Export]
    private Texture2D projectile { get; set; }
}
