using Godot;
using System;

public partial class BoardManager : TileMapLayer
{
    [Export(PropertyHint.ResourceType, "GameBoard")]
    public GameBoard GameBoard { get; set; }
    
    public override void _Ready()
    {
        GameBoard.AppleEaten += CreateNewApple;
        GameBoard.WallHit += ChangeSceneUponDeath;
        
        Tile[,] board = new Tile[GameInformation.TileMapSize.X, GameInformation.TileMapSize.Y];
        for (int i = 0; i < GameInformation.TileMapSize.X; i++)
        {
            for (int j = 0; j < GameInformation.TileMapSize.Y; j++)
            {
                if (i == 0 || i == GameInformation.TileMapSize.X - 1 || j == 0 || j == GameInformation.TileMapSize.Y - 1)
                    board[i, j] = new Tile(TileType.Wall);
                else board[i, j] = new Tile(TileType.Background);
            }
        }
        GameBoard.Board = board;
        
        CreateNewApple();
    }

    private void OnPlayerMovementAttempt(Vector2 position)
    {
        var boardPos = LocalToMap(position);
        if (!GameBoard.EatApple(boardPos))
            GameBoard.IsPositionBlocked(boardPos);
    }

    private Vector2I RandomLocation()
    {
        var random = new RandomNumberGenerator();
        return new Vector2I(
            random.RandiRange(1, GameInformation.TileMapSize.X - 2), 
            random.RandiRange(1, GameInformation.TileMapSize.Y - 2)
        );
    }
    
    private void CreateNewApple()
    { 
        Clear();
        var applePos = RandomLocation();
        GameBoard.Board[applePos.X, applePos.Y].Type = TileType.Apple;
        SetCell(applePos, 0, new Vector2I(0, 0));
    }

    private void ChangeSceneUponDeath() =>
        GetTree().ChangeSceneToFile(GameInformation.MainMenu);

    public override void _ExitTree()
    {
        GameBoard.AppleEaten -= CreateNewApple;
        GameBoard.WallHit -= ChangeSceneUponDeath;
    }
}
