using Godot;
using System;
using static Godot.Control;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 200;

    [Signal]
    public delegate void DiedEventHandler();

    int _tileSize = 16;

    Vector2 _startPosition;
    Vector2 _nextPosition;
    Vector2 _direction;

    public player()
    {
        _direction = new Vector2(0, -1);
    }
    public override void _Ready()
    {
        _startPosition = GlobalPosition;
        _nextPosition = GlobalPosition;
    }
    public override void _Input(InputEvent @event)
    {
        if (!(@event is InputEventMouse))
        {
            Vector2 TempDirecton = Input.GetVector("left", "right", "up", "down");

            if (TempDirecton != Vector2.Zero)
                _direction = TempDirecton;
        }
    }

    private void DirectionTimerTimeout()
    {
        _startPosition = _nextPosition;
        _nextPosition = (_direction * _tileSize) + _startPosition;

        var tween = CreateTween();
        var rot = _startPosition.AngleToPoint(_nextPosition) + Mathf.DegToRad(90);

        tween.TweenProperty(this, "rotation", rot, 0.25);
        tween.TweenProperty(this, "position", _nextPosition, 0.25);

        if (GetNode<RayCast2D>("RayCast2D").IsColliding())
            EmitSignal(SignalName.Died);
    }
}
