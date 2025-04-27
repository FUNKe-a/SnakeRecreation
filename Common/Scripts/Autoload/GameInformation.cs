using Godot;
using System;

public partial class GameInformation : Node
{
    public static Vector2I TileMapSize;
    public static int TileSize = 16;
    public static string MainMenu = @"res://Levels/MainMenu/MainMenu.tscn";
    public static Vector2 Resolution;
    
    public override void _Ready()
    {
        Resolution = GetViewport().GetVisibleRect().Size;
        var XBoundary = (int)Math.Ceiling(Resolution.X / 16f);
        var YBoundary = (int)Math.Ceiling(Resolution.Y / 16f);
       
        TileMapSize = new Vector2I(XBoundary, YBoundary);
    }
}
