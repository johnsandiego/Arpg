using Godot;

public partial class Game : Node2D
{
    private int currentLevel = 1;

    public override void _Ready()
    {
        LoadLevel(currentLevel);
    }

    private void LoadLevel(int level)
    {
        //foreach (Node child in GetChildren())
        //{
        //    if (child.Name.ToString().StartsWith("Level")) child.QueueFree();
        //}
        //var levelScene = GD.Load<PackedScene>($"res://Scenes/Level{level}.tscn");
        var testingScene = GD.Load<PackedScene>("res://Scenes/TestingScene.tscn");
        var levelNode = testingScene.Instantiate<Node2D>();
        AddChild(levelNode);
    }

    public void OnLevelCompleted()
    {
        currentLevel++;
        if (currentLevel > 10)
        {
            //GD.Print("Game Completed!");
            GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
        }
        else
        {
            LoadLevel(currentLevel);
        }
    }
}