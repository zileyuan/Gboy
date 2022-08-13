using Godot;

namespace HappySlime
{
    public class Exit : Area2D
    {
        [Export(PropertyHint.File, "*.tscn")] public string path = "";
        public override void _Ready()
        {
            this.Connect("body_entered", this, nameof(OnBodyEntered));
        }

        public void OnBodyEntered(Node node)
        {
            var globals = GetNode<Globals>("/root/Globals");
            if (path != "")
            {
                globals.GotoWorld(path);
            }
            else
            {
                globals.CompleteGame();
            }
        }
    }
}
