using Godot;
using System;
using System.Transactions;
using static Godot.Control;

public partial class Head : CharacterBody2D
{

    [Signal]
    public delegate void DiedEventHandler();

    int _tileSize = 16;

    Vector2 _direction;
    Vector2 _currentMove;

    public bool ObstacleInFront;

    PackedScene BodyPart = GD.Load<PackedScene>("res://Scenes/Player/SnakeBodyPart.tscn");

    public Head()
    {
        _direction = Vector2.Right;
        _currentMove = Vector2.Zero;
    }

    public override void _Ready()
    {
        _direction = _direction.Rotated(GetNode<Node2D>("..").Rotation).Round();
        _currentMove = _direction * _tileSize;
        GD.Print(_direction);
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

            //GD.Print(TempDirection);
        }
    }

    private void HeadMovementTimerTimeout()
    {
        var tween = CreateTween();
        var PreviousMove = _currentMove;
        _currentMove = _direction * _tileSize;

        //GD.Print(PreviousMove);
        //GD.Print(_currentMove);

        ////if game scene is starting finds proper player direction and prevents starting input
        //if (CurrentDirection == Vector2.Zero)
        //{
        //    var GlobalRaycastTarget = Position + GetNode<RayCast2D>("RayCast2D").TargetPosition;
        //    _direction = Position.DirectionTo(GlobalRaycastTarget).Rotated(-Rotation).Round();
        //    _direction.Y *= -1;
        //}

        var angle = PreviousMove.AngleTo(_currentMove);
        //GD.Print(angle);

        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            _currentMove = PreviousMove;
        }

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _currentMove, 0.25).AsRelative();
    }
}
