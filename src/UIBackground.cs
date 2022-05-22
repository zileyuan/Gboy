using Godot;

namespace Gboy
{
    public class UIBackground : ParallaxBackground
    {
        [Export] public float scrollVelocity = -30.0f;
        
        public override void _Ready()
        {
        
        }
        
        public override void _Process(float delta)
        {
            base._Process(delta);
            ScrollOffset = new Vector2(ScrollOffset.x + scrollVelocity * delta, ScrollOffset.y);
        }
    }
}
