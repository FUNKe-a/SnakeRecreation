using Godot;
using System;

public partial class ButtonsUi : CanvasLayer
{
    [Export(PropertyHint.File, "*.tscn")]
    public string PlayScene;

    OptionButton _options;

    public override void _Ready()
    {
        _options = GetNode<OptionButton>("VBoxContainer/OptionButton");
        var id = 2;
        if (GetWindow().Size == new Vector2(368, 256))
            id = 0;
        if (GetWindow().Size == new Vector2(736, 512))
            id = 1;
        if (GetWindow().Size == new Vector2(1104, 768))
            id = 2; 
        _options.Select(id);
    }
    
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
