using Godot;

namespace Gboy
{
    public class TitleScreen : Control
    {
        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("UI/Menus/MainMenu/OptionsButton").Connect("pressed", this, nameof(OptionsPressed));
            GetNode<Button>("UI/Menus/OptionsMenu/BackButton").Connect("pressed", this, nameof(BackPressed));
            GetNode<Button>("UI/Menus/OptionsMenu/BGMButton").Connect("pressed", this, nameof(BGMPressed));
            GetNode<Button>("UI/Menus/OptionsMenu/SFXButton").Connect("pressed", this, nameof(SFXPressed));
            GetNode<Button>("UI/Menus/MainMenu/StartButton").Connect("pressed", this, nameof(StartPressed));
            GetNode<Button>("UI/Menus/MainMenu/QuitButton").Connect("pressed", this, nameof(QuitPressed)); 
            var globals = GetNode<Globals>("/root/Globals");
            globals.LoadConfig();
            UpdateButton(globals);
        }

        public void StartPressed()
        {
            var globals = GetNode<Globals>("/root/Globals");
            globals.StartGame();
        }
        
        public void QuitPressed()
        {
            GetTree().Quit();
        }

        public void BGMPressed()
        {
            var globals = GetNode<Globals>("/root/Globals");
            globals.SetBGMEnabled(!globals.IsBGMEnabled());
            globals.SaveConfig();
            UpdateButton(globals);
        }
        
        public void SFXPressed()
        {
            var globals = GetNode<Globals>("/root/Globals");
            globals.SetSFXEnabled(!globals.IsSFXEnabled());
            globals.SaveConfig();
            UpdateButton(globals);
        }

        public void OptionsPressed()
        {
            GetNode<AnimationPlayer>("UI/Menus/Transitions").Play("show-options");
        }
        
        public void BackPressed()
        {
            GetNode<AnimationPlayer>("UI/Menus/Transitions").PlayBackwards("show-options");
        }

        public void UpdateButton(Globals globals)
        {
            GetNode<Button>("UI/Menus/OptionsMenu/BGMButton").Text = "音乐：" + (globals.IsBGMEnabled()?"关":"开");
            GetNode<Button>("UI/Menus/OptionsMenu/SFXButton").Text = "音效：" + (globals.IsSFXEnabled()?"关":"开");
        }

        
    }
}
