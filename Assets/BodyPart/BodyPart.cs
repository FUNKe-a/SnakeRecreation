using Godot;

public partial class BodyPart : Sprite2D
{
    public Timer MovementTimer { get; set; }
    public Player Connection { get; set; }
    public Vector2 PreviousMove { get; set; }

    public override void _Ready()
    {
        MovementTimer.Timeout += DirectionTimerTimeout;
    }

    public void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        
        var angle = PreviousMove.AngleTo(Connection.PreviousMove);

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", Connection.PreviousMove, 0.25).AsRelative();
        PreviousMove = Connection.PreviousMove;
    }

    public override void _ExitTree() =>
        MovementTimer.Timeout -= DirectionTimerTimeout;
}
