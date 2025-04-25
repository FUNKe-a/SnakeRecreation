using Godot;
using System;

public partial class BodyPart : Sprite2D
{
    public Player Connection { get; set; }
    public Vector2 PreviousMove { get; set; }
    
    public void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        PreviousMove = Connection.PreviousMove;
        
        var angle = PreviousMove.AngleTo(Connection.PreviousMove);

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", Connection.PreviousMove, 0.25).AsRelative();
    }

    public override void _ExitTree()
    {
        
    }
}
