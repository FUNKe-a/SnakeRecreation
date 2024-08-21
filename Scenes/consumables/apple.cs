using Godot;
using System;

public partial class apple : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(player))
            (body as player).EatApple();
    }
}