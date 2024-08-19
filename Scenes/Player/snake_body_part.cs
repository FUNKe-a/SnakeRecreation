using Godot;
using static Godot.GD;

public partial class snake_body_part : CharacterBody2D
{
    Vector2 _startPosition;
    Vector2 _nextPosition;

    public snake_body_part()
    {
        _startPosition = Vector2.Zero;
        _nextPosition = Vector2.Zero;
    }
    public snake_body_part(Vector2 StartPosition)
    {
        _startPosition = StartPosition;
        _nextPosition = Vector2.Zero;
    }


}
