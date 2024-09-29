using Godot;
using System;

public partial class apple : Area2D
{

    [Signal]
    public delegate void AppleEatenEventHandler();

    public override void _Ready()
    {
        AppleEaten += () => GetNode<Game>("../..").PlayerAppleEaten();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(SnakeHead))
            EmitSignal(SignalName.AppleEaten);
    }
}