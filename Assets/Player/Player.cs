using Godot;
using System;
using static Godot.Control;

public partial class Player : Sprite2D
{
    [Export(PropertyHint.File, "*.tscn")] 
    public string BodyPartScene;
    [Export(PropertyHint.ResourceType, "GameBoard")] 
    public GameBoard GameBoard;
    [Signal] public delegate void PlayerMovementAttemptEventHandler(Vector2 position);

    string _action;
    Vector2 _direction;
    Vector2 _currentMove;
    private int BodyPartCount;

    private PackedScene _packedBodyPart;
    
    public override void _Ready()
    {
        BodyPartCount = 0;
        _packedBodyPart = GD.Load<PackedScene>(BodyPartScene);
        _direction = CurrentPlayerDirection();
        _currentMove = _direction * GameInformation.TileSize;
        _action = string.Empty;
        
        GameBoard.ConnectToAppleEaten(AddBodyPart);
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
        GD.Print("Adding body part");
        var bodyPart = _packedBodyPart.Instantiate<BodyPart>();
        bodyPart.Name = $"BodyPart_{BodyPartCount++}";
        bodyPart.GlobalPosition = GlobalPosition - _currentMove;
        //bodyPart.Direction = _direction;
        //bodyPart.CurrentMove = _currentMove;
        GetNode<Node2D>("../BodyParts").AddChild(bodyPart);
    }

    public Vector2 CurrentPlayerDirection()
    {
        var Raycast = GetNode<RayCast2D>("RayCast2D");
        return Raycast.Position.DirectionTo(Raycast.TargetPosition);
    }

    private void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        var previousMove = _currentMove;

        _currentMove = _direction * GameInformation.TileSize;
        
        var angle = previousMove.AngleTo(_currentMove);

        //prevents player from going backwards
        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            _currentMove = previousMove;
        }

        EmitSignal(SignalName.PlayerMovementAttempt, _currentMove + Position);
        
        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", _currentMove, 0.25).AsRelative();
    }

    public override void _ExitTree()
    {
        GameBoard.DisconnectFromAppleEaten(AddBodyPart);
    }
}
