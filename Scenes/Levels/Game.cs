using Godot;
using System;
using System.Linq;

public partial class Game : Node2D
{
    [Export(PropertyHint.File)]
    public string MenuScene { get; set; }
    [Export(PropertyHint.File, "*.tscn")]
    public string AppleScene { get; set; }
    [Export(PropertyHint.File, "*.tscn")] 
    private string BodyPartScene { get; set; }

    private PackedScene _apple;
    private PackedScene _bodyPart;

    bool _isAppleEaten;

    public override void _Ready()
    {
        _isAppleEaten = false;
        _apple = GD.Load<PackedScene>(AppleScene);
        _bodyPart = GD.Load<PackedScene>(BodyPartScene);
        CreateNewApple();
    }

    private void PlayerDied()
    {
        GetTree().CallDeferred("change_scene_to_file", MenuScene);
    }

    private void CreateNewApple()
    {
        var AppleInst = _apple.Instantiate<apple>();
        AppleInst.Position = RandomLocation();
        AppleInst.ConnectToAppleEaten(PlayerAppleEaten);
        GetNode<Node2D>("Items").AddChild(AppleInst);
    }

    private void CreateNewBodyPart(float rotation, Vector2 oppositeDirection)
    {
        var BodyPartInst = _bodyPart.Instantiate<SnakeBodyPart>();
        BodyPartInst.Position = GetNode<SnakeHead>("SnakeHead").GlobalPosition + (oppositeDirection * 16);
        BodyPartInst.Rotation = rotation;
        GetNode<Node2D>("BodyParts").AddChild(BodyPartInst);
    }

    private void SnakeHeadPositionState(float rotation, Vector2 oppositeDirection)
    {
        if (_isAppleEaten)
            CreateNewBodyPart(rotation, oppositeDirection);

        _isAppleEaten = false;
    }

    public void PlayerAppleEaten()
    {
        CallDeferred("CreateNewApple");
        _isAppleEaten = true;
    }

    private Vector2I RandomLocation()
    {
        var random = new RandomNumberGenerator();
        return new Vector2I(random.RandiRange(1, 38) * 16 + 8, random.RandiRange(1, 28) * 16 + 8);
    }
}
