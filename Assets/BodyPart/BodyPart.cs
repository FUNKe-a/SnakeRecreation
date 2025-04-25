using Godot;
using System;

public partial class BodyPart : Sprite2D
{
    public Player Connection { get; set; }
    
    public void DirectionTimerTimeout()
    {
        var tween = CreateTween();

        tween.TweenProperty(this, "position", Connection.PreviousMove, 0.25).AsRelative();
    } 
}
