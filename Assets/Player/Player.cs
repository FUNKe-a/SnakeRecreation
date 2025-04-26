using Godot;
using System;
using static Godot.Control;

public partial class Player : Sprite2D, Body
{
    [Export(PropertyHint.File, "*.tscn")] 
    public string BodyPartScene;
    [Export(PropertyHint.ResourceType, "GameBoard")] 
    public GameBoard GameBoard;
    [Signal] public delegate void PlayerMovementAttemptEventHandler(Vector2 position);

    string _action;
    Vector2 _direction;
    Vector2 _currentMove;
    public Vector2 PreviousMove { get; set; }
    int BodyPartCount;

    private PackedScene _packedBodyPart;
    
    public override void _Ready()
    {
        BodyPartCount = 0;
        _packedBodyPart = GD.Load<PackedScene>(BodyPartScene);
        _direction = CurrentPlayerDirection();
        _currentMove = _direction * GameInformation.TileSize;
        _action = string.Empty;
        
        GameBoard.AppleEaten += AddBodyPart;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey)
        {
            if (@event.IsActionPressed("Up"))
                _direction = Vector2.Up;
            if (@event.IsActionPressed("Down"))
                _direction = Vector2.Down;
            if (@event.IsActionPressed("Left"))
                _direction = Vector2.Left;
            if (@event.IsActionPressed("Right"))
                _direction = Vector2.Right;
        }
    }

    public void AddBodyPart()
    {
        var bodyPart = _packedBodyPart.Instantiate<BodyPart>();
        var timer = GetNode<Timer>("DirectionTimer");
        Body lastPart = this;
        
        bodyPart.Name = $"BodyPart_{BodyPartCount}";
        if (BodyPartCount > 0)
            lastPart = GetNode<BodyPart>($"../BodyParts/BodyPart_{BodyPartCount - 1}");
        
        bodyPart.GlobalPosition = lastPart.GlobalPosition;
        bodyPart.Connection = lastPart;
        bodyPart.MovementTimer = timer;
        GetNode<Node2D>("../BodyParts").AddChild(bodyPart);

        BodyPartCount++;
    }

    public Vector2 CurrentPlayerDirection()
    {
        var Raycast = GetNode<RayCast2D>("RayCast2D");
        return Raycast.Position.DirectionTo(Raycast.TargetPosition);
    }

    private void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        PreviousMove = _currentMove;

        _currentMove = _direction * GameInformation.TileSize;
        
        var angle = PreviousMove.AngleTo(_currentMove);

        //prevents player from going backwards
        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            _currentMove = PreviousMove;
        }

        EmitSignal(SignalName.PlayerMovementAttempt, _currentMove + Position);
        
        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _currentMove, 0.25).AsRelative();
    }

    public override void _ExitTree() =>
        GameBoard.AppleEaten -= AddBodyPart;
}
