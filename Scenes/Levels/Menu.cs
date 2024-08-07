using Godot;

public partial class Menu : Node2D
{
    OptionButton _options;
    public readonly PackedScene GameLevel = ResourceLoader.Load<PackedScene>("res://Scenes/Levels/Game.tscn");

    public override void _Ready()
    {
        _options = GetNode<OptionButton>("UI/VBoxContainer/OptionButton");
    }
    private void OptionButtonItemSelected(int index)
    {
        var TempString = _options.GetItemText(index).Split('x');
        var StartupSize = new Vector2I(int.Parse(TempString[0]), int.Parse(TempString[1]));
        GetWindow().Size = StartupSize;
    }
    private void PlayButtonPressed()
    {
        GetTree().ChangeSceneToPacked(GameLevel);
    }
    private void ExitButtonPressed()
    {
        GetTree().Quit();
    }
}
