using Godot;
using System;

public partial class apple : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        if (body.GetType() == typeof(SnakeHead))
            body.GetNode<SnakeHead>("../SnakeHead").EatApple();
    }
}