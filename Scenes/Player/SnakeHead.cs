using Godot;
using System;
using static Godot.Control;

public partial class SnakeHead : CharacterBody2D
{
    [Signal]
    public delegate void DiedEventHandler();
    [Signal]
    public delegate void PositionStateEventHandler(float rotation, Vector2 oppositeDirection);

    string action;
    int _tileSize = 16;

    Vector2 _direction;
    Vector2 _currentMove;

    public SnakeHead()
    {
        _direction = Vector2.Zero;
        _currentMove = Vector2.Zero;
        action = string.Empty;
    }

    public override void _Ready()
    {
        _direction = CurrentPlayerDirection();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            if (@event.IsActionPressed("up"))
                _direction = Vector2.Up;
            if (@event.IsActionPressed("down"))
                _direction = Vector2.Down;
            if (@event.IsActionPressed("left"))
                _direction = Vector2.Left;
            if (@event.IsActionPressed("right"))
                _direction = Vector2.Right;
        }
    }

    public Vector2 CurrentPlayerDirection()
    {
        var Raycast = GetNode<RayCast2D>("RayCast2D");
        return Raycast.Position.DirectionTo(Raycast.TargetPosition);
    }

    private void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        var PreviousMove = _currentMove;

        _currentMove = _direction * _tileSize;

        var angle = PreviousMove.AngleTo(_currentMove);

        //prevents player from going backwards
        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            _currentMove = PreviousMove;
        }

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _currentMove, 0.25).AsRelative();
        tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.PositionState, this.Rotation, _direction * -1)));
    }

    private void BodyEnteredCollisionArea(Node2D body)
    {
        EmitSignal(SignalName.Died);
    }
}
