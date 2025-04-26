using Godot;
using System;

public partial class GameScene : Node2D
{
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionReleased("Exit"))
            GetTree().ChangeSceneToFile(GameInformation.MainMenu);
    }
}
