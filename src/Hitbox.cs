using Godot;

namespace Gboy
{
    public class Hitbox : Area2D
    {
        [Signal]
        public delegate void Hit();

        [Export] public bool instantKill;

        public override void _Ready()
        {
            this.Connect("area_entered", this, nameof(OnAreaEntered));
        }

        public void OnAreaEntered(Area2D hurtbox)
        {
            EmitSignal(nameof(Hit));
            hurtbox.EmitSignal(nameof(Hurtbox.Hurt), this);
        }
    }
}