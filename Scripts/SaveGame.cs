using Godot;

public partial class SaveGame : Node
{
    private const string SavePath = "user://savegame.tres";

    public void Save(PlayerStats stats)
    {
        ResourceSaver.Save(stats, SavePath);
    }

    public PlayerStats Load()
    {
        if (ResourceLoader.Exists(SavePath))
        {
            return ResourceLoader.Load<PlayerStats>(SavePath);
        }
        return new PlayerStats();
    }

    public override void _Ready()
    {
        // Make SaveGame a singleton
        if (!Engine.HasSingleton("SaveGame"))
        {
            Engine.RegisterSingleton("SaveGame", this);
        }
    }
}