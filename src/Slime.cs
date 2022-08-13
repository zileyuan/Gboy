using Godot;

namespace HappySlime
{
    public class Slime : Enemy
    {
        public enum Direction
        {
            Left = -1,
            Right = 1
        }

        [Export] public Direction direction = Direction.Left;

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            var wasOnWall = IsOnWall();
            var snap = Vector2.Down * 16;
            velocity = MoveAndSlideWithSnap(velocity, snap, Vector2.Up);

            if (IsOnWall() && !wasOnWall)
            {
                direction = (Direction)((int)(direction) * -1);
            }
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (isDead)
            {
                velocity.x = 0;
            }
            else
            {
                velocity = velocity.MoveToward(new Vector2(maxSpeed * (int)(direction), velocity.y), acceleration * delta);
            }

            velocity.y += Gravity * delta;
            if (velocity.x != 0)
            {
                GetNode<Sprite>("Sprite").FlipH = velocity.x > 0;
            }
        }
    }
}