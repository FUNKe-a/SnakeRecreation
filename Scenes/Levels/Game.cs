using Godot;
using System;
using System.Linq;

public partial class Game : Node2D
{
    [Export(PropertyHint.File)]
    string MenuScene = string.Empty;

    PackedScene Apple = GD.Load<PackedScene>("res://Scenes/consumables/apple.tscn");
    PackedScene BodyPart = GD.Load<PackedScene>("res://Scenes/Player/SnakeBodyPart.tscn");

    bool _isAppleEaten;

    public Game()
    {
        _isAppleEaten = false;
    }

    public override void _Ready()
    {
        CreateNewApple();
    }

    private void PlayerDied()
    {
        GetTree().CallDeferred("change_scene_to_file", MenuScene);
    }

    private void CreateNewApple()
    {
        var AppleInst = Apple.Instantiate<apple>();
        AppleInst.Position = RandomLocation();
        GetNode<Node2D>("Items").AddChild(AppleInst);
    }

    private void CreateNewBodyPart(float rotation)
    {
        var BodyPartInst = BodyPart.Instantiate<SnakeBodyPart>();
        BodyPartInst.Position = GetNode<SnakeHead>("SnakeHead").GlobalPosition;
        BodyPartInst.Rotation = rotation;
        GetNode<Node2D>("BodyParts").AddChild(BodyPartInst);
    }

    private void SnakeHeadPositionState(float rotation)
    {
        if (_isAppleEaten)
            CreateNewBodyPart(rotation);

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
