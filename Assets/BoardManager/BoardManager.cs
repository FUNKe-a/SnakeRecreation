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
            apple.Name = "Apple";
            apple.Visible = false;
            
            CallDeferred(MethodName.AddChild, apple);
            await ToSignal(apple, "tree_entered");
            
            bool isAppleOnSnake;
            do
            {
                var applePos = RandomLocation();
                apple.GlobalPosition = applePos;
                isAppleOnSnake = await apple.IsAppleOnSnake();
            } while (isAppleOnSnake);
            apple.Visible = true;
        }
        catch (Exception e)
        {
            GD.Print(e.Message);
        }
    }
}
