using Godot;
using System;

public partial class BoardManager : TileMapLayer
{
    [Export(PropertyHint.ResourceType, "GameBoard")]
    public GameBoard GameBoard { get; set; }
    
    public override void _Ready()
    {
        GameBoard.AppleEaten += CreateNewApple;
        GameBoard.PositionBlocked += ChangeSceneUponDeath;
        
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

    public void OnBodyPartMovementStarted(Vector2 position) =>
        GameBoard.UpdateSnakePosition(LocalToMap(position));

    public void OnBodyPartMovementFinished(Vector2 position) =>
        GameBoard.RemoveSnakePosition(LocalToMap(position));

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
        bool appleAdded = false;
        while (!appleAdded)
        {
            var applePos = RandomLocation();
            if (GameBoard.Board[applePos.X, applePos.Y].Type != TileType.Wall &&
                GameBoard.Board[applePos.X, applePos.Y].Type != TileType.Snake)
            {
                GameBoard.Board[applePos.X, applePos.Y].Type = TileType.Apple;
                SetCell(applePos, 0, new Vector2I(0, 0));
                appleAdded = true;
            }
        }
    }

    private void ChangeSceneUponDeath() =>
        GetTree().ChangeSceneToFile(GameInformation.MainMenu);

    public override void _ExitTree()
    {
        GameBoard.AppleEaten -= CreateNewApple;
        GameBoard.PositionBlocked -= ChangeSceneUponDeath;
    }
}
