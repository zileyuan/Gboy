using Godot;

namespace Gboy
{
    public class PauseMenu : Control
    {
        public override void _Ready()
        {
            GetNode<Button>("CenterContainer/VBoxContainer/ResumeButton")
                .Connect("pressed", this, nameof(OnResumeButtonPressed));
            GetNode<Button>("CenterContainer/VBoxContainer/QuitButton")
                .Connect("pressed", this, nameof(OnQuitButtonPressed));
            Connect("visibility_changed", this, nameof(OnVisibilityChanged));
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            if (Visible && @event.IsActionPressed("ui_cancel"))
            {
                HideMenu();
                GetTree().SetInputAsHandled();
            }
        }

        public void OnVisibilityChanged()
        {
            GetTree().Paused = Visible;
        }

        public void ShowMenu()
        {
            Show();
            GetNode<AnimationPlayer>("Transitions").Play("show");
        }

        public void HideMenu()
        {
            var transitions = GetNode<AnimationPlayer>("Transitions");
            transitions.PlayBackwards("show");
            ToSignal(transitions, "animation_finished");
            Hide();
        }

        public void OnResumeButtonPressed()
        {
            HideMenu();
        }

        public void OnQuitButtonPressed()
        {
            HideMenu();
            var globals = GetNode<Globals>("/root/Globals");
            globals.BackToTitle();
        }
    }
}
