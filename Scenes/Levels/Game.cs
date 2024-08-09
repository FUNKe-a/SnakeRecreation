using Godot;
using System;

public partial class Game : Node2D
{
    [Export(PropertyHint.File)]
    string MenuScene = string.Empty;
    private void PlayerDied()
    {
        GetTree().ChangeSceneToFile(MenuScene);
    }
}
