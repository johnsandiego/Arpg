// GameManager.cs (New)
using Godot;
using System.Collections.Generic;

public partial class GameManager : Node2D
{
    [Export] private PackedScene enemyScene;
    [Export] private float spawnInterval = 2f;
    [Export] private float spawnRadius = 350f;
    [Export] private bool useRandomRadius = false;
    [Export] private float minRadius = 100f;
    [Export] private float maxRadius = 200f;
    private float spawnTimer = 0f;
    private int waveNumber = 1;
    private Player player;
    private List<Enemy> activeEnemies = new List<Enemy>();
    private int score = 0;

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
    }

    public override void _Process(double delta)
    {
        spawnTimer -= (float)delta;
        if (spawnTimer <= 0)
        {
            SpawnWave();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnWave()
    {
        int enemyCount = waveNumber * 2;
        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemy = enemyScene.Instantiate<Enemy>();
            Vector2 spawnPos = GetRandomSpawnPosition();
            enemy.Position = spawnPos;
            AddChild(enemy);
            activeEnemies.Add(enemy);
        }
        waveNumber++;
    }

    //private Vector2 GetRandomSpawnPosition()
    //{
    //    TileMapLayer viewportSize = GetTree().CurrentScene.GetNode<TileMapLayer>("Level1/Background");
    //    if (viewportSize == null)
    //    {
    //        GD.PrintErr("Viewport size not found!");
    //        return Vector2.Zero;
    //    }
    //    float radius = viewportSize.Scale.Length() / 2 + 100;
    //    GD.Print("Radius: " + radius);
    //    float angle = (float)GD.RandRange(0, Mathf.Pi * 2);
    //    player = GetTree().Root.GetNode<Player>("Game/Player");
    //    if (player == null)
    //    {
    //        GD.PrintErr("Player node not found!");
    //        return Vector2.Zero;
    //    }
    //    return player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    //}

    private Vector2 GetRandomSpawnPosition()
    {
        TileMapLayer viewportSize = GetTree().CurrentScene.GetNode<TileMapLayer>("Level1/Background");
        if (viewportSize == null)
        {
            //GD.PrintErr("Viewport size not found!");
            return Vector2.Zero;
        }

        // Calculate final radius based on parameters
        float finalRadius;
        if (useRandomRadius)
        {
            // Ensure minRadius is not larger than maxRadius
            float min = Mathf.Min(minRadius, maxRadius);
            float max = Mathf.Max(minRadius, maxRadius);
            finalRadius = (float)GD.RandRange(min, max);
        }
        else
        {
            finalRadius = spawnRadius;
        }

        //GD.Print("Final Spawn Radius: " + finalRadius);
        float angle = (float)GD.RandRange(0, Mathf.Pi * 2);

        player = GetTree().Root.GetNode<Player>("Game/Player");
        if (player == null)
        {
            //GD.PrintErr("Player node not found!");
            return Vector2.Zero;
        }

        return player.Position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * finalRadius;
    }

    public void EnemyDefeated()
    {
        score += 10 * waveNumber;
    }

}