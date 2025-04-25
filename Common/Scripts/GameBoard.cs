using System;
using Godot;

[GlobalClass]
public partial class GameBoard : Resource
{
    [Signal]
    public delegate void AppleEatenEventHandler();
    
    public Tile[,] Board { get; set; }

    /// <summary>
    /// updates the board to delete apple at the given location
    /// and deletes the old apple from the board.
    /// </summary>
    /// <returns>Whether apple was eaten</returns>
    public bool EatApple(Vector2I location)
    {
        if (Board[location.X, location.Y].Type == TileType.Apple)
        {
            Board[location.X, location.Y].Type = TileType.Background;
            EmitSignal(SignalName.AppleEaten);
            return true;
        }
        
        return false;
    }
    
    public void ConnectToAppleEaten(Action func) =>
        AppleEaten += () => func();
    
    public void DisconnectFromAppleEaten(Action func) =>
        AppleEaten -= () => func();
}
