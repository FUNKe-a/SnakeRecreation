using Godot;
using System;

public interface Body
{
    Vector2 GlobalPosition { get; set; }
    Vector2 PreviousPosition { get; set; }
}
