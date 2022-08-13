using Godot;

namespace HappySlime
{
    public class HUD : CanvasLayer
    {
        public override void _Ready()
        {
            GetNode<TextureButton>("HBoxContainer/PauseButton").Connect("pressed", this, nameof(OnPausePressed));
            var label = GetNode<Label>("HBoxContainer/CoinLabel");
            var globals = GetNode<Globals>("/root/Globals");
            globals.Connect(nameof(Globals.CoinsChanged), this, nameof(OnCoinsChanged));
            
            label.Text = globals.GetCoins().ToString();
        }

        public void OnCoinsChanged()
        {
            var label = GetNode<Label>("HBoxContainer/CoinLabel");
            var globals = GetNode<Globals>("/root/Globals");
            label.Text = globals.GetCoins().ToString();
        }

        public void OnPausePressed()
        {
            GetNode<PauseMenu>("PauseMenu").ShowMenu();
        }
    }
}
