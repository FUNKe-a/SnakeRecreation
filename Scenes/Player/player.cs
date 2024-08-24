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

        //var pog = BodyPart.Instantiate<SnakeBodyPart>();
        //pog.SetVariables(_startPosition, _nextPosition);
        //GetNode<Node2D>("BodyParts").CallDeferred("add_child", pog);
    }

    private void PlayerMovementTimerTimeout()
    {
        if (_obstacleInFront && _obstacleCheck)
        {
            EmitSignal(SignalName.Died);
        }

        if (_obstacleInFront)
            _obstacleCheck = true;

        if (GetNode<RayCast2D>("Head/RayCast2D").IsColliding())
            _obstacleInFront = true;
        else
        {
            _obstacleInFront = false;
            _obstacleCheck   = false;
        }
    }
}
