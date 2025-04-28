using Godot;
using System;
using System.Reflection;

public partial class BoardManager : TileMapLayer
{
    [Export(PropertyHint.File, "*.tscn")] 
    public string ConsumableItemScene;

    private PackedScene _consumableItemScene;
    private RandomNumberGenerator _random;
    
    public override void _Ready()
    {
        _random = new RandomNumberGenerator();
        _random.Randomize();
        _consumableItemScene = GD.Load<PackedScene>(ConsumableItemScene);
        GetNode<Player>("Player").PlayerAppleEaten += CreateNewApple;
        CreateNewApple();
    }

    public Vector2I RandomLocation()
    {
        return new Vector2I(
            _random.RandiRange(1, GameInformation.TileMapSize.X - 2),
            _random.RandiRange(1, GameInformation.TileMapSize.Y - 2)
        ) * GameInformation.TileSize + new Vector2I(GameInformation.TileSize / 2, GameInformation.TileSize / 2);
    }
    
    private async void CreateNewApple()
    {
        try
        {
            var apple = _consumableItemScene.Instantiate<Apple>();
            var applePos = RandomLocation();
            apple.GlobalPosition = applePos;
            CallDeferred(MethodName.AddChild, apple);
            await ToSignal(GetTree(), "process_frame");
            while (apple.IsAppleOnSnakeBodyPart())
            {
                applePos = RandomLocation();
                apple.GlobalPosition = applePos;
            }
        }
        catch (Exception e)
        {
            GD.Print(e.Message);
        }
    }

    private void DeleteOldApple(Apple apple) =>
        apple.QueueFree();

    private void ChangeSceneUponDeath() =>
        GetTree().ChangeSceneToFile(GameInformation.MainMenu);
}
