using Godot;
using System;

public partial class ButtonsUi : CanvasLayer
{
    [Export(PropertyHint.File, "*.tscn")]
    public string PlayScene;

    OptionButton _options;
    
    public override void _Ready() =>
        _options = GetNode<OptionButton>("VBoxContainer/OptionButton");
    
    private void OnOptionButtonItemSelected(int index)
    {
        var tempString = _options.GetItemText(index).Split('x');
        var startupSize = new Vector2I(int.Parse(tempString[0]), int.Parse(tempString[1]));
        GetWindow().Size = startupSize; 
    }
    
    private void OnExitButtonUp() =>
        GetTree().Quit();
    
    private void OnPlayButtonUp() =>
        GetTree().ChangeSceneToFile(PlayScene);
}
