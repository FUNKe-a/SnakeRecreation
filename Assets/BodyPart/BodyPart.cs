using Godot;

public partial class BodyPart : Sprite2D, Body
{
    public Timer MovementTimer { get; set; }
    public Body Connection { get; set; }
    public Vector2 PreviousPosition { get; set; }

    public override void _Ready() =>
        MovementTimer.Timeout += DirectionTimerTimeout;

    public void DirectionTimerTimeout()
    {
        var tween = CreateTween();

        tween.TweenProperty(this, "rotation", 0, 0.25);
        tween.TweenProperty(this, "global_position", Connection.PreviousPosition, 0.25);
        PreviousPosition = GlobalPosition;
    }

    public override void _ExitTree() =>
        MovementTimer.Timeout -= DirectionTimerTimeout;
}
