using Godot;
using System;

//for inheritance only
public interface Body
{
    Vector2 GlobalPosition { get; set; }
    Vector2 PreviousPosition { get; set; }
}
