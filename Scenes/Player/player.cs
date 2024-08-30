using Godot;
using System;

public partial class Player : Node2D
{
    [Signal]
    public delegate void AppleEatenEventHandler();
    [Signal]
    public delegate void DiedEventHandler();

    bool _obstacleInFront;
    bool _obstacleCheck;

    public Player()
    {
        _obstacleInFront = false;
    }

    public void EatApple()
    {
        EmitSignal(SignalName.AppleEaten);
    }

    private void PlayerMovementTimerTimeout()
    {
    }

    private void BodyEnteredCollisionArea(Node2D body)
    {
        if (body.GetType() == typeof(TileMapLayer))
            EmitSignal(SignalName.Died);
    }
}
