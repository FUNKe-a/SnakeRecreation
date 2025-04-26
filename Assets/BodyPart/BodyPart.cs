using Godot;

public partial class BodyPart : Area2D, Body
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

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player playerNode)
            GetTree().CallDeferred("change_scene_to_file", GameInformation.MainMenu);

    }

    public override void _ExitTree() =>
        MovementTimer.Timeout -= DirectionTimerTimeout;
}
