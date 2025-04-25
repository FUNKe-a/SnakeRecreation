using Godot;

public partial class Tile : RefCounted
{
    public TileType Type { get; set; }

    public Tile() => Type = TileType.None;
    public Tile(TileType type) => Type = type;
}
