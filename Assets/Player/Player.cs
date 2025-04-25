using Godot;
using System;
using static Godot.Control;

public partial class Player : Sprite2D
{
    [Export(PropertyHint.ResourceType, "GameBoard")] 
    public GameBoard GameBoard;
    [Signal] public delegate void PlayerMovementAttemptEventHandler(Vector2 position);

    string _action;
    Vector2 _direction;
    Vector2 _currentMove;

    public override void _Ready()
    {
        _direction = CurrentPlayerDirection();
        _currentMove = _direction * GameInformation.TileSize;
        _action = string.Empty;
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
        var previousMove = _currentMove;

        _currentMove = _direction * GameInformation.TileSize;
        
        var angle = previousMove.AngleTo(_currentMove);

        //prevents player from going backwards
        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            _currentMove = previousMove;
        }

        EmitSignal(SignalName.PlayerMovementAttempt, _currentMove + Position);
        
        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _currentMove, 0.25).AsRelative();
        //tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.PositionState, this.Rotation, _direction * -1));
    }
}
