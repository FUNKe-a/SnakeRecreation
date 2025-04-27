using Godot;
using System;

public partial class Apple : StaticBody2D
{
    private Area2D _tileCheck;
    
    public override void _Ready() =>
        _tileCheck = GetNode<Area2D>("Area2D");

    public bool IsAppleOnSnakeBodyPart() =>
        _tileCheck.HasOverlappingAreas() || _tileCheck.HasOverlappingBodies();
}
