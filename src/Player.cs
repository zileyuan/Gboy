using Godot;

namespace Gboy
{
    public class Player : KinematicBody2D
    {
        [Export] public bool isDead;
        private const float MaxSpeed = 350;
        private const float JumpForce = 880;
        private const float Gravity = 2000;
        private float Acceleration => (float)(MaxSpeed / 0.2);
        private float AirAcceleration => (float)(MaxSpeed / 0.05);
        public Vector2 _velocity = Vector2.Zero;
        private bool _isJumping;

        public override void _Ready()
        {
            base._Ready();
            GD.Print("Player Ready ...");
            GetNode<Timer>("TrailTimer").Connect("timeout", this, nameof(OnTrailTimerTimeout));
            GetNode<Hurtbox>("Hurtbox").Connect(nameof(Hurtbox.Hurt), this, nameof(OnHurt));
            GetNode<Hitbox>("Hitbox").Connect(nameof(Hitbox.Hit), this, nameof(OnHit));
        }

        public async void OnHurt(Hitbox hitbox)
        {
            // return;
            _velocity.y = -JumpForce;
            var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            animationPlayer.Play("death");
            await ToSignal(animationPlayer, "animation_finished");
            var globals = GetNode<Globals>("/root/Globals");
            globals.ReloadWorld();
        }

        public void OnHit() 
        {
            _velocity.y = -JumpForce / 2;
            if (!IsOnFloor())
            {
                TweenScale(new Vector2(1.25f, 1/1.25f));
            }
        }

        public void OnTrailTimerTimeout()
        {
            if (_velocity.x != 0 && ResourceLoader.Load("res://scenes/Trail.tscn") is PackedScene packedScene)
            {
                var trail = packedScene.Instance();
                GetParent().AddChild(trail);
                GetParent().MoveChild(trail, GetIndex());
                string[] properties =
                {
                    "hframes",
                    "vframes",
                    "frame",
                    "texture",
                    "flip_h",
                    "global_position"
                };
                var sprite = GetNode<Sprite>("Sprite");
                foreach (var name in properties)
                {
                    trail.Set(name, sprite.Get(name));
                }
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            if (!isDead)
            {
                if (@event.IsActionPressed("jump"))
                {
                    GetNode<Timer>("JumpRequestTimer").Start();
                }

                if (@event.IsActionReleased("jump") && _velocity.y < -JumpForce / 2)
                {
                    _velocity.y = -JumpForce / 2;
                }
            }
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            GD.Print($"_Process {delta}");
            _velocity.y += Gravity * delta;

            if (!isDead)
            {
                var direction = Input.GetActionRawStrength("move_right") - Input.GetActionRawStrength("move_left");
                var acc = Acceleration;
                if (!IsOnFloor())
                {
                    acc = AirAcceleration;
                }

                _velocity = _velocity.MoveToward(new Vector2(direction * MaxSpeed, _velocity.y), acc * delta);

                var canJump = IsOnFloor() || GetNode<Timer>("CoyoteTimer").TimeLeft > 0;
                if (canJump && GetNode<Timer>("JumpRequestTimer").TimeLeft > 0)
                {
                    _velocity.y = -JumpForce;
                    _isJumping = true;
                    GetNode<Timer>("JumpRequestTimer").Stop();
                    GetNode<Timer>("CoyoteTimer").Stop();
                    GetNode<AudioStreamPlayer>("JumpSound").Play();
                    TweenScale(new Vector2(1/1.25f, 1.25f));
                }

                if (_isJumping)
                {
                    GetNode<AnimationPlayer>("AnimationPlayer").Play("jump");
                }
                else if (_velocity.x == 0)
                {
                    GetNode<AnimationPlayer>("AnimationPlayer").Play("idle");
                }
                else
                {
                    GetNode<AnimationPlayer>("AnimationPlayer").Play("walk");
                }

                GetNode<Sprite>("Sprite").FlipH = direction < 0;
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            var wasOnFloor = IsOnFloor();
            var snap = Vector2.Zero;
            if (!_isJumping)
            {
                snap = Vector2.Down * 16;
            }

            _velocity = MoveAndSlideWithSnap(_velocity, snap, Vector2.Up);
            if (IsOnFloor())
            {
                _isJumping = false;
                if (!wasOnFloor)
                {
                    TweenScale(new Vector2(1.25f, 1/1.25f));
                }
            }
            else if (wasOnFloor && !_isJumping)
            {
                GetNode<Timer>("CoyoteTimer").Start();
            }
        }

        public void TweenScale(Vector2 target)
        {
            var sprite = GetNode<Sprite>("Sprite");
            var tween = GetNode<Tween>("Tween");
            tween.InterpolateProperty(sprite, "scale", null, target, 
                0.05f, Tween.TransitionType.Sine, Tween.EaseType.InOut);
            tween.InterpolateProperty(sprite, "scale", target, Vector2.One, 
                0.1f, Tween.TransitionType.Sine, Tween.EaseType.InOut, 0.05f);
            tween.Start();
        }
    }
}