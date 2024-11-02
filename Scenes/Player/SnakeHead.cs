using Godot;
using System;
using static Godot.Control;

public partial class SnakeHead : CharacterBody2D
{
    [Signal]
    public delegate void DiedEventHandler();
    [Signal]
    public delegate void PositionStateEventHandler(float rotation);

    int _tileSize = 16;

    Vector2 _direction;
    Vector2 _currentMove;

    public SnakeHead()
    {
        _direction = Vector2.Zero;
        _currentMove = Vector2.Zero;
    }

    public override void _Input(InputEvent @event)
    {
        if (!(@event is InputEventMouse))
        {
            Vector2 TempDirection = Input.GetVector("left", "right", "up", "down");

            if (TempDirection.X != 0 && TempDirection.Y != 0)
                TempDirection = Vector2.Zero;

            if (TempDirection != Vector2.Zero)
                _direction = TempDirection;
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

        //if game scene is starting finds proper player direction and prevents starting input
        if (PreviousMove == Vector2.Zero)
            _direction = CurrentPlayerDirection();

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
        tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.PositionState, this.Rotation)));
    }

    private void BodyEnteredCollisionArea(Node2D body)
    {
        EmitSignal(SignalName.Died);
    }
}
