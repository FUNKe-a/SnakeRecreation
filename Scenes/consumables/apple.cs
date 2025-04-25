using Godot;
using System;

public partial class apple : Area2D
{

    [Signal]
    public delegate void AppleEatenEventHandler();

    public void ConnectToAppleEaten(Action func) =>
        AppleEaten += () => func();

    private void OnBodyEntered(Node2D body)
    {
        Console.WriteLine(body);
        if (body.GetType() == typeof(SnakeHead))
            EmitSignal(SignalName.AppleEaten);
        this.QueueFree();
    }
}