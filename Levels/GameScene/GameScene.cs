using Godot;
using System;
using System.Linq;

public partial class GameScene : Node2D
{
    [Export(PropertyHint.File)]
    public string MenuScene { get; set; }

    private void PlayerDied() =>
        GetTree().CallDeferred("change_scene_to_file", MenuScene);
}
