using Godot;
using System;
using static Godot.Control;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 200;


    int _tileSize = 16;
    Vector2 _nextPosition;

    Vector2 _direction;
    bool _isMoving;

    public player()
    {
        _direction = new Vector2(0, -1);
        _isMoving = false;
    }
    public override void _Ready()
    {
        _nextPosition = (_direction * _tileSize) + GlobalPosition;
    }
    public override void _Input(InputEvent @event)
    {
        if (!_isMoving)
            _direction = Input.GetVector("left", "right", "up", "down") == new Vector2(0, 0) ? Input.GetVector("left", "right", "up", "down") : _direction;
    }

    private void DirectionTimerTimeout()
    {
        if (!GetNode<RayCast2D>("RayCast2D").IsColliding())
        {
            _isMoving = true;
            var tween = CreateTween();
            tween.TweenProperty(this, "position", _nextPosition, 0.5);
            tween.TweenCallback(Callable.From(() => _isMoving = false));
            GD.Print(_direction);
            _nextPosition = (_direction * _tileSize) + GlobalPosition;
        }
    }
}
