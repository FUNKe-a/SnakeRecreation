using Godot;
using System;

public partial class Apple : Area2D
{
    public bool IsAppleOnSnakeBodyPart() =>
        HasOverlappingAreas() || HasOverlappingBodies();
}
