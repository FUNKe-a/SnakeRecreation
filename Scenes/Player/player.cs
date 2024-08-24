using Godot;
using System;
using static Godot.Control;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 200;

    [Signal]
    public delegate void AppleEatenEventHandler();
    [Signal]
    public delegate void DiedEventHandler();

    int _tileSize = 16;

    Vector2 _startPosition;
    Vector2 _nextPosition;
    Vector2 _direction;

    bool _obstacleInFront;

    PackedScene BodyPart = GD.Load<PackedScene>("res://Scenes/Player/snake_body_part.tscn");

    public player()
    {
        _direction       = Vector2.Zero;
        _obstacleInFront = false;
    }

    public override void _Ready()
    {
        _startPosition = GlobalPosition;
        _nextPosition  = GlobalPosition;
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

    public void EatApple()
    {
        EmitSignal(SignalName.AppleEaten);
    }

    private void DirectionTimerTimeout()
    {
        var tween            = CreateTween();
        var CurrentDirection = _startPosition.DirectionTo(_nextPosition);

        if (GetNode<RayCast2D>("RayCast2D").IsColliding())
            _obstacleInFront = true;

        if (_obstacleInFront && CurrentDirection == _direction)
            EmitSignal(SignalName.Died);
        else _obstacleInFront = false;

        //if game scene is starting finds proper player direction and prevents starting input
        if (CurrentDirection == Vector2.Zero)
        {
            var GlobalRaycastTarget = GlobalPosition + GetNode<RayCast2D>("RayCast2D").TargetPosition;
            _direction = GlobalPosition.DirectionTo(GlobalRaycastTarget).Rotated(-Rotation).Round();
            _direction.Y *= -1;
        }

        //calculate the current and the next position
        _startPosition = _nextPosition;
        _nextPosition  = (_direction * _tileSize) + _startPosition;

        var angle = CurrentDirection.AngleTo(_direction);

        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle         = 0;
            _direction    = CurrentDirection;
            _nextPosition = (CurrentDirection * _tileSize) + _startPosition;
        }

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _nextPosition, 0.25);
    }
}
