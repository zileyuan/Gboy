using Godot;

namespace HappySlime
{
    public class Hurtbox : Area2D
    {
        [Signal]
        public delegate void Hurt(Hitbox hitbox);
    }
}