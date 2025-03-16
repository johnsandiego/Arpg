using Godot;

public partial class UI : CanvasLayer
{
    [Export]
    public ProgressBar hpBar;
    [Export]
    public ProgressBar manaBar;
    [Export]
    public Label levelLabel;
    [Export]
    public Label uiScore;
    [Export]
    public Label uiWave;
    [Export]
    public Player player;

    public override void _Ready()
    {
        hpBar = GetNode<ProgressBar>("HealthBar");
        manaBar = GetNode<ProgressBar>("ManaBar");
        levelLabel = GetNode<Label>("Level");
        uiScore = GetNode<Label>("Score");
        uiWave = GetNode<Label>("Wave");
        player = GetTree().Root.GetNode<Player>("Game/Player");
    }

    public override void _Process(double delta)
    {
        hpBar.Value = player.Stats.CurrentHP;
        hpBar.MaxValue = player.Stats.MaxHP;
        manaBar.Value = player.Stats.CurrentMana;
        manaBar.MaxValue = player.Stats.MaxMana;
        levelLabel.Text = $"Level: {player.Stats.Level}";
    }


}