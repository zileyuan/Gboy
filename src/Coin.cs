using Godot;

namespace Gboy
{
    public class Coin : Area2D
    {
        public override void _Ready()
        {
            this.Connect("body_entered", this, nameof(BodyEntered));
        }

        public void BodyEntered(Node body)
        {
            GetNode<AnimationPlayer>("AnimationPlayer").Play("picked");
            var globals = GetNode<Globals>("/root/Globals");
            globals.SetCoinPending(globals.CoinPending + 1);
        }
    }
}