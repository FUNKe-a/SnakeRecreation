using Godot;
using System;

public partial class BoardManager : TileMapLayer
{
    [Export(PropertyHint.File, "*.tscn")] 
    public string ConsumableItemScene;

    private PackedScene _consumableItemScene;
    
    public override void _Ready()
    {
        _consumableItemScene = GD.Load<PackedScene>(ConsumableItemScene);
        GetNode<Player>("Player").PlayerAppleEaten += CreateNewApple;
        CreateNewApple();
    }

    public Vector2I RandomLocation()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();
        
        return new Vector2I(
            random.RandiRange(16, (GameInformation.TileMapSize.X - 1) * 16),
            random.RandiRange(16, (GameInformation.TileMapSize.Y + 1) * 16)
        ).Snapped(new Vector2I(16, 16)) + new Vector2I(GameInformation.TileSize / 2, GameInformation.TileSize / 2);
    }
    
    private void CreateNewApple()
    {
        var apple = _consumableItemScene.Instantiate<Apple>();
        var applePos = RandomLocation();
        apple.GlobalPosition = applePos;
        AddChild(apple);
        while (apple.IsAppleOnSnakeBodyPart())
        {
            applePos = RandomLocation();
            apple.GlobalPosition = applePos;
        }
    }

    private void DeleteOldApple(Apple apple) =>
        apple.QueueFree();

    private void ChangeSceneUponDeath() =>
        GetTree().ChangeSceneToFile(GameInformation.MainMenu);
}
