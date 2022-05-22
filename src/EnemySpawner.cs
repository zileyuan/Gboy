using Godot;

namespace Gboy
{
    public class EnemySpawner : Position2D
    {
        [Export] public PackedScene enemyScene;
        [Export] public float interval;

        public override void _Ready()
        {
            base._Ready();
            var timer = GetNode<Timer>("Timer");
            timer.WaitTime = interval;
            timer.Connect("timeout", this, nameof(OnTimerTimeout));
            if (enemyScene != null)
            {
                timer.Start();
            }
        }

        public void OnTimerTimeout()
        {
            var enemy = enemyScene.Instance<Enemy>();
            GetParent().AddChild(enemy);
            enemy.GlobalPosition = GlobalPosition;
        }
    }
}