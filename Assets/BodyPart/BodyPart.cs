using Godot;

public partial class BodyPart : Sprite2D, Body
{
    [Signal] public delegate void PlayerEnteredBodyPartEventHandler();
    [Signal] public delegate void MovementStartedEventHandler(Vector2 position);
    [Signal] public delegate void MovementFinishedEventHandler(Vector2 position); 
    
    public Timer MovementTimer { get; set; }
    public Body Connection { get; set; }
    public Vector2 PreviousPosition { get; set; }

    public override void _Ready()
    {
        MovementStarted += (position) => GetNode<BoardManager>("../../../BoardManager").OnBodyPartMovementStarted(position);
        MovementFinished += (position) => GetNode<BoardManager>("../../../BoardManager").OnBodyPartMovementFinished(position);
        MovementTimer.Timeout += DirectionTimerTimeout;
    }

    public void DirectionTimerTimeout()
    {
        EmitSignal(SignalName.MovementFinished, GlobalPosition);
        var tween = CreateTween();
        
        tween.TweenProperty(this, "rotation", 0, 0.15);
        tween.TweenProperty(this, "global_position", Connection.PreviousPosition, 0.15);
        tween.TweenCallback(Callable.From(() => EmitSignal(SignalName.MovementStarted, GlobalPosition)));
        PreviousPosition = GlobalPosition;
    }

    private void OnBodyEntered(Node2D body)
    {
        //if (body is Player playerNode)
            //GetTree().CallDeferred("change_scene_to_file", GameInformation.MainMenu);

    }

    public override void _ExitTree() =>
        MovementTimer.Timeout -= DirectionTimerTimeout;
}
