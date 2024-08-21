using Godot;
using System;
using System.Linq;

public partial class Game : Node2D
{
    [Export(PropertyHint.File)]
    string MenuScene = string.Empty;

    PackedScene Apple = GD.Load<PackedScene>("res://Scenes/consumables/apple.tscn");

    public override void _Ready()
    {
        CreateNewApple();
    }

    private void PlayerDied()
    {
        GetTree().ChangeSceneToFile(MenuScene);
    }
    private void CreateNewApple()
    {
        var AppleInst = Apple.Instantiate() as Area2D;
        AppleInst.Position = RandomLocation();
        GetNode<Node2D>("Items").AddChild(AppleInst);
    }

    private void PlayerAppleEaten()
    {
        foreach (var child in GetNode<Node2D>("Items").GetChildren())
            child.QueueFree();

        CallDeferred("CreateNewApple", null);
    }

    private Vector2I RandomLocation()
    {
        var random = new RandomNumberGenerator();
        return new Vector2I(random.RandiRange(1, 38) * 16 + 8, random.RandiRange(1, 28) * 16 + 8);
    }
}
