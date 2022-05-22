using Godot;

namespace Gboy
{
    public class Hurtbox : Area2D
    {
        [Signal]
        public delegate void Hurt(Hitbox hitbox);
    }
}