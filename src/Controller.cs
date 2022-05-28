using Godot;

namespace Gboy
{
    public class Controller : CanvasLayer
    {
        private Globals _globals;

        public override void _Ready()
        {
            _globals = GetNode<Globals>("/root/Globals");

            var leftButton = GetNode<TouchScreenButton>("LeftButton");
            leftButton.Connect("pressed", this, nameof(OnLeftButtonDown));
            leftButton.Connect("released", this, nameof(OnLeftButtonUp));

            var rightButton = GetNode<TouchScreenButton>("RightButton");
            rightButton.Connect("pressed", this, nameof(OnRightButtonDown));
            rightButton.Connect("released", this, nameof(OnRightButtonUp));
        }

        public void OnLeftButtonDown() 
        {
            var direction = -1;
            _globals.EmitSignal(nameof(Globals.DirectionChanged), direction);
        }

        public void OnLeftButtonUp() 
        {
            var direction = 0;
            _globals.EmitSignal(nameof(Globals.DirectionChanged), direction);
        }

        public void OnRightButtonDown() 
        {
            var direction = 1;
            _globals.EmitSignal(nameof(Globals.DirectionChanged), direction);
        }

        public void OnRightButtonUp() 
        {
            var direction = 0;
            _globals.EmitSignal(nameof(Globals.DirectionChanged), direction);
        }
    }
}

