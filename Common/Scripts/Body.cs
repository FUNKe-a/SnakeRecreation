using Godot;
using System;

//for inheritance only
public interface Body
{
    Vector2 PreviousMove { get; set; }
    Vector2 GlobalPosition { get; set; }
}
