using Godot;
using static Godot.GD;

public partial class SnakeBodyPart : CharacterBody2D
{
    Vector2 _startPosition;
    Vector2 _nextPosition;

    public SnakeBodyPart()
    {
        _startPosition = Vector2.Zero;
        _nextPosition  = Vector2.Zero;
    }

    public void SetVariables(Vector2 StartPosition, Vector2 NextPosition)
    {
        _startPosition = StartPosition;
        _nextPosition  = NextPosition;
    }
}
