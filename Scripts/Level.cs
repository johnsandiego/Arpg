using Godot;

public partial class Level : Node2D
{
    private int enemyCount = 0;

    public override void _Ready()
    {
        var spawners = GetNode("Spawners");
        for(var i = 0; i < 1; i++) 
        {
            if (spawners is Node2D position)
            {
                var enemyScene = GD.Load<PackedScene>("res://Scenes/Enemy.tscn");
                var enemy = enemyScene.Instantiate<Enemy>();
                enemy.GlobalPosition = position.GlobalPosition;
                AddChild(enemy);
            }
        }

        //foreach (var enemy in GetTree().GetNodesInGroup("enemies"))
        //{
        //    enemy.Connect("EnemyDied", new Callable(this, nameof(OnEnemyDied)));
        //    enemyCount++;
        //}
    }

    public override void _Process(double delta)
    {
        //if(GetTree().GetNodesInGroup("enemies").Count == 0)
        //{
        //    GetNode<Game>("../Game").OnLevelCompleted();
        //}
    }

    private void OnEnemyDied()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            GetNode("/root/Game").Call("OnLevelCompleted");
        }
    }
}
