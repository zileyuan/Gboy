using Godot;

namespace Gboy
{
    public class ClickSound : AudioStreamPlayer
    {
        public override void _Ready()
        {
            HookButtonSound(GetParent());
        }
        
        public void HookButtonSound(Node node)
        {
            foreach (var child in node.GetChildren())
            {
                if (child is Button)
                {
                    (child as Button).Connect("pressed", this, "play");
                }
                else
                {
                    HookButtonSound(child as Node);
                }
            }
        }
    }
}
