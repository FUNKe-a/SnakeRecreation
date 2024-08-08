using Godot;
using System;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed { get; set; } = 200;

    public Vector2 _direction;

    public player()
    {
        _direction = new Vector2(0, -1);
    }
    public override void _Input(InputEvent @event)
    {
        int DegreesToRotate = 999;

        if (@event.IsActionPressed("up"))
        {
            DegreesToRotate = 0;
            _direction = new Vector2(0, -1);
        }
        if (@event.IsActionPressed("down"))
        {
            DegreesToRotate = 180;
            _direction = new Vector2(0, 1);
        }
        if (@event.IsActionPressed("left"))
        {
            DegreesToRotate = -90;
            _direction = new Vector2(-1, 0);
        }
        if (@event.IsActionPressed("right"))
        {
            DegreesToRotate = 90;
            _direction = new Vector2(1, 0);
        }

        if (DegreesToRotate != 999 && Math.Abs((int)RotationDegrees - DegreesToRotate) != 180)
            RotationDegrees = DegreesToRotate;
    }

    public override void _Process(double delta)
    {
        Velocity = _direction * Speed;
        MoveAndSlide();
    }

    private void DirectionTimerTimeout()
    {
        //_direction = (GetNode<RayCast2D>("RayCast2D").Position - GetNode<RayCast2D>("RayCast2D").TargetPosition).Normalized();
        //GD.Print(_direction);
    }
}
