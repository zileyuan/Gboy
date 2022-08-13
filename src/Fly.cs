using Godot;

namespace HappySlime
{
    public class Fly : Enemy
    {
        private Vector2? _targetPosition;

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            velocity = MoveAndSlide(velocity);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (isDead)
            {
                velocity.x = 0;
                velocity.y += Gravity * delta;
            }
            else
            {
                _targetPosition = CalcTargetPosition();
                if (_targetPosition == null)
                {
                    velocity = velocity.MoveToward(Vector2.Zero, acceleration * delta);
                }
                else
                {
                    var direction = GlobalPosition.DirectionTo(_targetPosition.Value);
                    velocity = velocity.MoveToward(direction * maxSpeed, acceleration * delta);
                    GetNode<Sprite>("Sprite").FlipH = direction.x > 0;
                }
            }
        }

        private Vector2? CalcTargetPosition()
        {
            var bodies = GetNode<Area2D>("PlayerSensor").GetOverlappingBodies();
            if (bodies.Count > 0)
            {
                return ((Node2D)bodies[0]).GlobalPosition + Vector2.Up * 50;
            }

            if (_targetPosition != null && GlobalPosition.DistanceSquaredTo(_targetPosition.Value) < 25)
            {
                return Vector2.Zero;
            }

            return _targetPosition;
        }
    }
}