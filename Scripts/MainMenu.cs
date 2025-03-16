using Godot;

public partial class MainMenu : Control
{
    public void OnStartPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    public void OnOptionPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/OptionMenu.tscn");
    }

    public void OnQuitPressed()
    {
        GetTree().Quit();
    }
}