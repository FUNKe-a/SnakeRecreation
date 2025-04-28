using Godot;

public partial class BodyPart : AnimatableBody2D, Body
{
    [Signal]
    public delegate void PlayerEnteredBodyPartEventHandler();
    public Timer MovementTimer { get; set; }
    public Body Connection { get; set; }
    public Vector2 PreviousPosition { get; set; }

    public override void _Ready() =>
        MovementTimer.Timeout += DirectionTimerTimeout;

    public void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        
        tween.TweenProperty(this, "rotation", 0, 0.15);
        tween.TweenProperty(this, "global_position", Connection.PreviousPosition, 0.15);
        PreviousPosition = GlobalPosition;
    }

    public void DisableCollision() =>
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
}
