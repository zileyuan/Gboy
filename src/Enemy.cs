using Godot;

namespace HappySlime
{
    public class Enemy : KinematicBody2D
    {
        [Export] public bool isDead;
        [Export] public float maxSpeed = 50;
        [Export] public float acceleration = (float)(50 / 0.2);

        protected const float Gravity = 2000;
        protected Vector2 velocity = Vector2.Zero;

        public override void _Ready()
        {
            base._Ready();
            GetNode<Hurtbox>("Hurtbox").Connect(nameof(Hurtbox.Hurt), this, nameof(OnHurt));
            GetNode<VisibilityEnabler2D>("VisibilityEnabler2D")
                .Connect("viewport_entered", this, nameof(OnViewportEntered));
        }

        public void OnViewportEntered(Viewport viewport)
        {
            var enabler = GetNode<VisibilityEnabler2D>("VisibilityEnabler2D");
            enabler.ProcessParent = false;
            enabler.PhysicsProcessParent = false;
            enabler.PauseAnimations = false;
        }

        public void OnHurt(Hitbox hitbox)
        {
            var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            if (!hitbox.instantKill && animationPlayer.HasAnimation("death"))
            {
                animationPlayer.Play("death");
            }
            else
            {
                QueueFree();
            }
        }
    }
}