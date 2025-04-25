using Godot;
using System;

public partial class BodyPart : Sprite2D
{
    public Vector2 Direction { get; set; }
    public Vector2 CurrentMove { get; set; }
    
    private void DirectionTimerTimeout()
    {
        var tween = CreateTween();
        var previousMove = CurrentMove;

        CurrentMove = Direction * GameInformation.TileSize;
        
        var angle = previousMove.AngleTo(CurrentMove);

        //prevents player from going backwards
        if (Math.Abs(angle) > Mathf.DegToRad(90))
        {
            angle = 0;
            CurrentMove = previousMove;
        }

        tween.TweenProperty(this, "rotation", angle, 0.25).AsRelative();
        tween.TweenProperty(this, "position", CurrentMove, 0.25).AsRelative();
    } 
}
